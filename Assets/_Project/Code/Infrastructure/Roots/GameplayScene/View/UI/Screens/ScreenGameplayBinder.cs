using UnityEngine;
using VVM.UI;

namespace Infrastructure.Roots.GameplayScene.View.UI.Screens
{
	public sealed class ScreenGameplayBinder : WindowBinder<ScreenGameplayViewModel>
	{
		[SerializeField] public GameplayButton _bGoToMainMenuScene;
		[SerializeField] public GameplayButton _bGoToGarageSceneScene;

		private void OnEnable()
		{
			_bGoToMainMenuScene.Button.onClick.AddListener(OnGoToMainMenuClicked);
			_bGoToGarageSceneScene.Button.onClick.AddListener(OnGoToGameplaySceneClicked);
		}

		private void OnDisable()
		{
			_bGoToMainMenuScene.Button.onClick.RemoveListener(OnGoToMainMenuClicked);
			_bGoToGarageSceneScene.Button.onClick.RemoveListener(OnGoToGameplaySceneClicked);
		}

		private void OnGoToMainMenuClicked()
		{
			_viewModel.RequestGoToMainMenuScene();
		}

		private void OnGoToGameplaySceneClicked()
		{
			_viewModel.RequestGoToGarageScene();
		}
	}
}