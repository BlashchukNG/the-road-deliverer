using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIAnimations.Buttons
{
	public sealed class AnimatedButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
	{
		private Sequence _sequenceClick;
		private Sequence _sequenceEnter;


		private void Awake()
		{
			_sequenceClick = DOTween
			                 .Sequence()
			                 .SetLink(gameObject)
			                 .Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1), 0.05f))
			                 .Append(transform.DOScale(new Vector3(1.3f, 1.3f, 1), 0.05f))
			                 .Append(transform.DOScale(Vector3.one, 0.05f))
			                 .Pause();

			_sequenceEnter = DOTween
			                 .Sequence()
			                 .SetLink(gameObject)
			                 .Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1), 0.05f))
			                 .Append(transform.DOScale(new Vector3(.98f, 0.98f, 1), 0.05f))
			                 .Append(transform.DOScale(Vector3.one, 0.05f))
			                 .Pause();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			_sequenceClick.Restart();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_sequenceEnter.Restart();
		}
	}
}