using DG.Tweening;
using UIAnimations;
using UnityEngine;
using Utils;
using VVM.UI;

namespace Infrastructure.Roots.MainMenuScene.Views.UI.PopupSettings
{
	public sealed class PopupSettingsBinder : PopupBinder<PopupSettingsViewModel>
	{
		[SerializeField] private CanvasGroupShowHideController _vignette;
		[SerializeField] private UIFrame _uiFrame;

		protected override void Start()
		{
			base.Start();

			DOTween.Sequence()
			       .SetLink(gameObject)
			       .AppendCallback(_vignette.Show)
			       .AppendInterval(_vignette.Duration)
			       .AppendCallback(_uiFrame.Show);
		}

		public override void Close()
		{
			DOTween.Sequence()
			       .SetLink(gameObject)
			       .AppendCallback(_uiFrame.Hide)
			       .AppendInterval(_uiFrame.Duration)
			       .AppendCallback(_vignette.Hide)
			       .AppendInterval(_vignette.Duration)
			       .AppendCallback(base.Close);
		}
	}
}