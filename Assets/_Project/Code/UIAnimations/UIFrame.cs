using DG.Tweening;
using UnityEngine;

namespace UIAnimations
{
	public sealed class UIFrame : MonoBehaviour
	{
		[SerializeField] public RectTransform _rect, _buttonCloseRect;
		[SerializeField] public CanvasGroup _contentCanvasGroup;

		[SerializeField] public float _durationChangeSize, _durationFade, _durationButtonCloseShow, _minValue, _endValue;
		[SerializeField] public bool _isHorizontal;

		private Sequence _sequenceShow;
		private Sequence _sequenceHide;

		private void Awake()
		{
			_contentCanvasGroup.alpha = 0;
			_rect.SetSizeWithCurrentAnchors(_isHorizontal ? RectTransform.Axis.Horizontal : RectTransform.Axis.Vertical, _minValue);

			_buttonCloseRect.anchoredPosition = new Vector2(-125, 0);

			_sequenceShow = DOTween
			                .Sequence()
			                .SetLink(gameObject)
			                .Append(_contentCanvasGroup.DOFade(1, _durationFade))
			                .AppendCallback(ButtonCloseShow)
			                .Pause();

			_sequenceHide = DOTween
			                .Sequence()
			                .SetLink(gameObject)
			                .Append(_contentCanvasGroup.DOFade(0, _durationFade))
			                .AppendCallback(SetHideState)
			                .AppendCallback(ButtonCloseHide)
			                .AppendCallback(() => DOVirtual.Float(_endValue, _minValue, _durationChangeSize,
				                x => _rect.SetSizeWithCurrentAnchors(_isHorizontal ? RectTransform.Axis.Horizontal : RectTransform.Axis.Vertical, x)))
			                .Pause();
		}

		private void SetHideState()
		{
			_contentCanvasGroup.alpha = 0;
			_rect.SetSizeWithCurrentAnchors(_isHorizontal ? RectTransform.Axis.Horizontal : RectTransform.Axis.Vertical, _minValue);
		}

		private void ButtonCloseShow() => _buttonCloseRect.DOAnchorPosX(0, _durationButtonCloseShow);
		private void ButtonCloseHide() => _buttonCloseRect.DOAnchorPosX(-125, _durationButtonCloseShow);


		public void Show()
		{
			SetHideState();

			DOVirtual.Float(_minValue, _endValue, _durationChangeSize,
				         x => _rect.SetSizeWithCurrentAnchors(_isHorizontal ? RectTransform.Axis.Horizontal : RectTransform.Axis.Vertical, x))
			         .OnComplete(() => _sequenceShow.Restart());
		}

		public void Hide()
		{
			_sequenceHide.Restart();
		}
	}
}