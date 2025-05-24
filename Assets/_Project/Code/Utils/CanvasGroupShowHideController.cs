using Constants;
using DG.Tweening;
using UnityEngine;

namespace Utils
{
	[RequireComponent(typeof(CanvasGroup))]
	public sealed class CanvasGroupShowHideController : MonoBehaviour
	{
		private CanvasGroup _canvasGroup;

		private void Awake()
		{
			TryGetComponent(out _canvasGroup);

			_canvasGroup.alpha = 0;
		}


		public void Show()
		{
			_canvasGroup.DOFade(1, InfrastructureConstants.SHOW_HIDE_LOADING_SCREEN_DURATION);
		}

		public void Hide()
		{
			_canvasGroup.DOFade(0, InfrastructureConstants.SHOW_HIDE_LOADING_SCREEN_DURATION);
		}
	}
}