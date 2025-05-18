using System;
using Constants;
using DG.Tweening;
using UnityEngine;

namespace Utils
{
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(CanvasGroup))]
	public class CanvasShowHideController : MonoBehaviour
	{
		private Canvas _canvas;
		private CanvasGroup _canvasGroup;

		private void Awake()
		{
			TryGetComponent(out _canvas);
			TryGetComponent(out _canvasGroup);
		}


		public void Show()
		{
			_canvasGroup.alpha = 0;
			_canvas.enabled = true;
			
			_canvasGroup.DOFade(1, InfrastructureConstants.SHOW_HIDE_LOADING_SCREEN_DURATION);
		}

		public void Hide()
		{
			_canvasGroup.DOFade(0, InfrastructureConstants.SHOW_HIDE_LOADING_SCREEN_DURATION)
			            .OnComplete(() =>
			            {
				            _canvasGroup.alpha = 0;
				            _canvas.enabled = false;
			            });
		}
	}
}