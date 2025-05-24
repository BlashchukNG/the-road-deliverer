using Constants;
using Infrastructure.DI;
using Infrastructure.Roots.GarageScene.Views;
using Infrastructure.Roots.GarageScene.Views.UI.Screens;
using R3;
using VVM.Root;

namespace Infrastructure.Roots.GarageScene.Services.UI
{
	public sealed class GarageUIService : UIService
	{
		private readonly Subject<Unit> _exitSceneToMainMenuRequest;
		private readonly Subject<Unit> _exitSceneToGameplayRequest;

		public GarageUIService(DIContainer diContainer) : base(diContainer)
		{
			_exitSceneToMainMenuRequest = _diContainer.Resolve<Subject<Unit>>(SignalTags.EXIT_TO_MAIN_MENU_SCENE_REQUEST);
			_exitSceneToGameplayRequest = _diContainer.Resolve<Subject<Unit>>(SignalTags.EXIT_TO_GAMEPLAY_SCENE_REQUEST);
		}

		public ScreenGarageViewModel OpenMainScreen()
		{
			var viewModel = new ScreenGarageViewModel(_diContainer, _exitSceneToMainMenuRequest, _exitSceneToGameplayRequest);
			var rootUI = _diContainer.Resolve<UIGarageRootViewModel>();
			rootUI.OpenScreen(viewModel);
			return viewModel;
		}
	}
}