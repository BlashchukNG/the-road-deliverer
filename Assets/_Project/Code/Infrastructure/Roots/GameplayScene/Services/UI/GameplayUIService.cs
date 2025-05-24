using Constants;
using Infrastructure.DI;
using Infrastructure.Roots.GameplayScene.View;
using Infrastructure.Roots.GameplayScene.View.UI.Screens;
using R3;
using VVM.Root;

namespace Infrastructure.Roots.GameplayScene.Services.UI
{
	public sealed class GameplayUIService : UIService
	{
		private readonly Subject<Unit> _exitSceneToMainMenuRequest;
		private readonly Subject<Unit> _exitSceneToGarageRequest;

		public GameplayUIService(DIContainer diContainer) : base(diContainer)
		{
			_exitSceneToMainMenuRequest = _diContainer.Resolve<Subject<Unit>>(SignalTags.EXIT_TO_MAIN_MENU_SCENE_REQUEST);
			_exitSceneToGarageRequest = _diContainer.Resolve<Subject<Unit>>(SignalTags.EXIT_TO_GARAGE_SCENE_REQUEST);
		}

		public ScreenGameplayViewModel OpenMainScreen()
		{
			var viewModel = new ScreenGameplayViewModel(this, _exitSceneToMainMenuRequest, _exitSceneToGarageRequest);
			var rootUI = _diContainer.Resolve<UIGameplayRootViewModel>();
			rootUI.OpenScreen(viewModel);
			return viewModel;
		}
	}
}