using DG.Tweening;
using UnityEngine;

namespace Logic.InteractableEntities.Doors
{
	public sealed class Door : MonoBehaviour
	{
		[SerializeField] private Transform _partLeft;
		[SerializeField] private Transform _partRight;
		[SerializeField] private float _targetLeftX, _targetRightX;
		[SerializeField] private float _duration = 1;

		private float _defLeftX;
		private float _defRightX;
		
		public bool isBusy;


		private void Awake()
		{
			_defLeftX = _partLeft.localPosition.x;
			_defRightX = _partRight.localPosition.x;
		}

		public void SetAction(bool toOpen)
		{
			if (isBusy) return;

			isBusy = true;
			if (toOpen)
			{
				_partLeft.DOLocalMoveX(_targetLeftX, _duration)
				         .SetEase(Ease.Linear)
				         .OnComplete(() => isBusy = false);
				_partRight.DOLocalMoveX(_targetRightX, _duration)
				          .SetEase(Ease.Linear);
			}
			else
			{
				_partLeft.DOLocalMoveX(_defLeftX, _duration)
				         .SetEase(Ease.Linear)
				         .OnComplete(() => isBusy = false);
				_partRight.DOLocalMoveX(_defRightX, _duration)
				          .SetEase(Ease.Linear);
			}
		}
	}
}