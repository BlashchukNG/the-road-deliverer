using DG.Tweening;
using UnityEngine;

namespace Utils
{
	[RequireComponent(typeof(CanvasGroup))]
	public sealed class CanvasGroupShowHideController : MonoBehaviour
	{
		public float Duration => _duration;

		[SerializeField] private float _duration = 0.5f;

		private CanvasGroup _canvasGroup;

		private void Awake()
		{
			TryGetComponent(out _canvasGroup);

			_canvasGroup.alpha = 0;
		}


		public void Show()
		{
			_canvasGroup.DOFade(1, _duration);
		}

		public void Hide()
		{
			_canvasGroup.DOFade(0, _duration);
		}
	}
}