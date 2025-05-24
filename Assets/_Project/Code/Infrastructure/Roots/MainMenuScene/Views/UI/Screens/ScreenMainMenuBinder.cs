using UnityEngine;
using VVM.UI;

namespace Infrastructure.Roots.MainMenuScene.Views.UI.Screens
{
	public sealed class ScreenMainMenuBinder : WindowBinder<ScreenMainMenuViewModel>
	{
		[SerializeField] public MainMenuButton _bPopopSettings;
		[SerializeField] public MainMenuButton _bGoToGarageScene;

		private void OnEnable()
		{
			_bPopopSettings.Button.onClick.AddListener(OnPopopSettingsClicked);
			_bGoToGarageScene.Button.onClick.AddListener(OnGoToGarageSceneClicked);
		}

		private void OnDisable()
		{
			_bPopopSettings.Button.onClick.RemoveListener(OnPopopSettingsClicked);
			_bGoToGarageScene.Button.onClick.RemoveListener(OnGoToGarageSceneClicked);
		}

		private void OnPopopSettingsClicked()
		{
			_viewModel.RequestOpenSettingsPopup();
		}

		private void OnGoToGarageSceneClicked()
		{
			_viewModel.RequestGoToGarageScene();
		}
	}
}