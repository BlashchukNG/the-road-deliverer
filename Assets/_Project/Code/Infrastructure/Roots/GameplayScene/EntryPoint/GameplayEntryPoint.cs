using Constants;
using Infrastructure.DI;
using Infrastructure.Roots.AppRoot;
using Infrastructure.Roots.AppRoot.Services.AssetInstantiate;
using Infrastructure.Roots.GameplayScene.EnterExitParams;
using Infrastructure.Roots.GameplayScene.Registrations;
using Infrastructure.Roots.GameplayScene.Services.UI;
using Infrastructure.Roots.GameplayScene.View;
using Infrastructure.Roots.GarageScene.EnterExitParams;
using Infrastructure.Roots.MainMenuScene.EnterExitParams;
using R3;
using UnityEngine;

namespace Infrastructure.Roots.GameplayScene.EntryPoint
{
	public sealed class GameplayEntryPoint : MonoBehaviour
	{
		[SerializeField] private UIGameplayRootBinder _uiRootBinderPrefab;
		//[SerializeField] private WorldGameplayRootBinder _prefabWorldRootBinder;

		private DIContainer _diContainer;
		private DIContainer _viewsDIContainer;

		public Observable<GameplayExitParams> Run(DIContainer diContainer, GameplayEnterParams enterParams)
		{
			_diContainer = diContainer;
			GameplayRegistrations.Register(_diContainer, enterParams);
			_viewsDIContainer = new DIContainer(_diContainer);
			GameplayViewModelRegistrations.Register(_viewsDIContainer);
			
			var sceneUI = _diContainer.Resolve<IAssetInstantiateService>().GetInstance(_uiRootBinderPrefab);
			_diContainer.Resolve<UIRootView>().AttachSceneUI(sceneUI.gameObject);

			InitWorld();
			InitUI();

			Debug.Log($"Entering main menu entry point: {enterParams?.DebugData}");
			
			return CreateExitSignal();
		}
		
		private Observable<GameplayExitParams> CreateExitSignal()
		{
			var mainMenuEnterParams = new MainMenuEnterParams("from gameplay");
			var garageEnterParams = new GarageEnterParams("from gameplay");

			var exitToMainMenuSignalSubject = _diContainer.Resolve<Subject<Unit>>(SignalTags.EXIT_TO_MAIN_MENU_SCENE_REQUEST);
			var exitToGarageSignalSubject = _diContainer.Resolve<Subject<Unit>>(SignalTags.EXIT_TO_GARAGE_SCENE_REQUEST);

			var exitToMainMenuParams = new GameplayExitParams(mainMenuEnterParams);
			var exitToGarageParams = new GameplayExitParams(garageEnterParams);

			var exitSignal = exitToMainMenuSignalSubject
			                 .Select(_ => exitToMainMenuParams)
			                 .Merge(exitToGarageSignalSubject.Select(_ => exitToGarageParams));

			return exitSignal;
		}

		private void InitWorld()
		{
			
		}

		private void InitUI()
		{
			var uiRoot = _diContainer.Resolve<UIRootView>();
			var uiSceneRootBinder = Instantiate(_uiRootBinderPrefab);
			uiRoot.AttachSceneUI(uiSceneRootBinder.gameObject);

			var uiSceneRootViewModel = _viewsDIContainer.Resolve<UIGameplayRootViewModel>();
			uiSceneRootBinder.Bind(uiSceneRootViewModel);

			var uiService = _viewsDIContainer.Resolve<GameplayUIService>();
			uiService.OpenMainScreen();
		}
	}
}