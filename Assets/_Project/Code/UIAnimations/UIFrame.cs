using DG.Tweening;
using UnityEngine;

namespace UIAnimations
{
	public sealed class UIFrame : MonoBehaviour
	{
		public float Duration => _durationChangeSize + _durationButtonCloseShow;

		[SerializeField] private RectTransform _rect, _buttonCloseRect;
		[SerializeField] private CanvasGroup _contentCanvasGroup;

		[SerializeField] private float _durationChangeSize, _durationFade, _durationButtonCloseShow, _minValue, _endValue;
		[SerializeField] private bool _isHorizontal;

		private Sequence _sequenceShow;
		private Sequence _sequenceHide;
		private float _defPositionY;

		private void Awake()
		{
			_defPositionY = _rect.anchoredPosition.y;

			_contentCanvasGroup.alpha = 0;
			_rect.SetSizeWithCurrentAnchors(_isHorizontal ? RectTransform.Axis.Horizontal : RectTransform.Axis.Vertical, _minValue);

			_buttonCloseRect.anchoredPosition = new Vector2(-125, 0);

			_sequenceShow = DOTween
			                .Sequence()
			                .SetLink(gameObject)
			                .Append(_contentCanvasGroup.DOFade(1, _durationFade))
			                .Append(_rect.DOAnchorPosY(_defPositionY, _durationFade))
			                .Append(DOVirtual.Float(_minValue, _endValue, _durationChangeSize, ResizeFrame))
			                .AppendInterval(_durationChangeSize)
			                .Append(_buttonCloseRect.DOAnchorPosX(0, _durationButtonCloseShow))
			                .Pause();


			_sequenceHide = DOTween
			                .Sequence()
			                .SetLink(gameObject)
			                .AppendInterval(0.2f)
			                .Append(_buttonCloseRect.DOAnchorPosX(-125, _durationButtonCloseShow))
			                .Append(DOVirtual.Float(_endValue, _minValue, _durationChangeSize, ResizeFrame))
			                .Append(_rect.DOAnchorPosY(-900, _durationFade))
			                .AppendInterval(_durationFade*2)
			                .Append(_contentCanvasGroup.DOFade(0, _durationFade))
			                .AppendCallback(SetHideState)
			                .Pause();
			
			SetHideState();
		}

		private void ResizeFrame(float x)
		{
			_rect.SetSizeWithCurrentAnchors(_isHorizontal ? RectTransform.Axis.Horizontal : RectTransform.Axis.Vertical, x);
		}

		private void SetHideState()
		{
			_contentCanvasGroup.alpha = 0;
			_rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, -900);
			_rect.SetSizeWithCurrentAnchors(_isHorizontal ? RectTransform.Axis.Horizontal : RectTransform.Axis.Vertical, _minValue);
		}

		public void Show() => _sequenceShow.Restart();
		public void Hide() => _sequenceHide.Restart();
	}
}