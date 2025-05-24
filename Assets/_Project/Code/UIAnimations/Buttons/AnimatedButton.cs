using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIAnimations.Buttons
{
	public sealed class AnimatedButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
	{
		private bool _clicked;

		public void OnPointerClick(PointerEventData eventData)
		{
			DOTween
				.Sequence()
				.SetLink(gameObject)
				.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1), 0.05f))
				.Append(transform.DOScale(new Vector3(1.3f, 1.3f, 1), 0.1f))
				.Append(transform.DOScale(Vector3.one, 0.05f));
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (_clicked) return;
			_clicked = true;
			DOVirtual.DelayedCall(1f, () => _clicked = false);

			DOTween
				.Sequence()
				.SetLink(gameObject)
				.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1), 0.05f))
				.Append(transform.DOScale(new Vector3(.98f, 0.98f, 1), 0.1f))
				.Append(transform.DOScale(Vector3.one, 0.05f));
		}
	}
}