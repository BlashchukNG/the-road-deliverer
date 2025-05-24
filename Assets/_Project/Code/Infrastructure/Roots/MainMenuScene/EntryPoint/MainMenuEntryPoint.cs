using Constants;
using Infrastructure.DI;
using Infrastructure.Roots.AppRoot;
using Infrastructure.Roots.GarageScene.EnterExitParams;
using Infrastructure.Roots.MainMenuScene.EnterExitParams;
using Infrastructure.Roots.MainMenuScene.Registrations;
using Infrastructure.Roots.MainMenuScene.Services.UI;
using Infrastructure.Roots.MainMenuScene.Views;
using R3;
using UnityEngine;

namespace Infrastructure.Roots.MainMenuScene.EntryPoint
{
	public sealed class MainMenuEntryPoint : MonoBehaviour
	{
		[SerializeField] private UIMainMenuRootBinder _uiRootBinderPrefab;

		private DIContainer _diContainer;
		private DIContainer _viewsDIContainer;

		public Observable<MainMenuExitParams> Run(DIContainer diContainer, MainMenuEnterParams enterParams)
		{
			_diContainer = diContainer;
			MainMenuRegistrations.Register(_diContainer, enterParams);
			_viewsDIContainer = new DIContainer(_diContainer);
			MainMenuViewModelRegistrations.Register(_viewsDIContainer);

			InitWorld();
			InitUI();

			Debug.Log($"Entering main menu entry point: {enterParams?.DebugData}");

			return CreateExitSignal();
		}

		private Observable<MainMenuExitParams> CreateExitSignal()
		{
			var exitParams = new MainMenuExitParams(new GarageEnterParams("from main menu"));
			var exitSceneRequest = _diContainer.Resolve<Subject<Unit>>(SignalTags.EXIT_TO_MAIN_MENU_SCENE_REQUEST);
			var exitToGarageSignal = exitSceneRequest.Select(_ => exitParams);
			return exitToGarageSignal;
		}

		private void InitWorld()
		{
		}

		private void InitUI()
		{
			var uiRoot = _diContainer.Resolve<UIRootView>();
			var uiSceneRootBinder = Instantiate(_uiRootBinderPrefab);
			uiRoot.AttachSceneUI(uiSceneRootBinder.gameObject);

			var uiSceneRootViewModel = _viewsDIContainer.Resolve<UIMainMenuRootViewModel>();
			uiSceneRootBinder.Bind(uiSceneRootViewModel);

			var uiService = _viewsDIContainer.Resolve<MainMenuUIService>();
			uiService.OpenMainScreen();
		}
	}
}