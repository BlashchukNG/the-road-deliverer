using UnityEngine;
using VVM.UI;

namespace Infrastructure.Roots.GarageScene.Views.UI.Screens
{
	public sealed class ScreenGarageBinder : WindowBinder<ScreenGarageViewModel>
	{
		[SerializeField] public GarageButton _bGoToMainMenuScene;
		[SerializeField] public GarageButton _bGoToGameplaySceneScene;

		private void OnEnable()
		{
			_bGoToMainMenuScene.Button.onClick.AddListener(OnGoToMainMenuClicked);
			_bGoToGameplaySceneScene.Button.onClick.AddListener(OnGoToGameplaySceneClicked);
		}

		private void OnDisable()
		{
			_bGoToMainMenuScene.Button.onClick.RemoveListener(OnGoToMainMenuClicked);
			_bGoToGameplaySceneScene.Button.onClick.RemoveListener(OnGoToGameplaySceneClicked);
		}

		private void OnGoToMainMenuClicked()
		{
			_viewModel.RequestGoToMainMenuScene();
		}

		private void OnGoToGameplaySceneClicked()
		{
			_viewModel.RequestGoToGameplayScene();
		}
	}
}