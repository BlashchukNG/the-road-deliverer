using Constants;
using Infrastructure.DI;
using Infrastructure.Roots.MainMenuScene.Views;
using Infrastructure.Roots.MainMenuScene.Views.UI.PopupSettings;
using Infrastructure.Roots.MainMenuScene.Views.UI.ScreenMainMenu;
using R3;
using VVM.Root;

namespace Infrastructure.Roots.MainMenuScene.Services.UI
{
	public sealed class MainMenuUIService : UIService
	{
		private readonly Subject<Unit> _exitSceneRequest;

		public MainMenuUIService(DIContainer diContainer) : base(diContainer)
		{
			_exitSceneRequest = _diContainer.Resolve<Subject<Unit>>(SignalTags.EXIT_SCENE_REQUEST);
		}

		public ScreenMainMenuViewModel OpenMainScreen()
		{
			var viewModel = new ScreenMainMenuViewModel(this, _exitSceneRequest);
			var rootUI = _diContainer.Resolve<UIMainMenuRootViewModel>();
			rootUI.OpenScreen(viewModel);
			return viewModel;
		}

		public PopupSettingsViewModel OpenPopupSettings()
		{
			var popup = new PopupSettingsViewModel();
			var rootUI = _diContainer.Resolve<UIMainMenuRootViewModel>();
			rootUI.OpenPopup(popup);
			return popup;
		}
	}
}