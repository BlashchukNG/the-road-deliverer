using Constants;
using Infrastructure.DI;
using Infrastructure.Roots.AppRoot;
using Infrastructure.Roots.AppRoot.Services.AssetInstantiate;
using Infrastructure.Roots.GameplayScene.EnterExitParams;
using Infrastructure.Roots.GarageScene.EnterExitParams;
using Infrastructure.Roots.GarageScene.Registrations;
using Infrastructure.Roots.GarageScene.Services.UI;
using Infrastructure.Roots.GarageScene.ViewModels;
using Infrastructure.Roots.GarageScene.Views;
using Infrastructure.Roots.MainMenuScene.EnterExitParams;
using Infrastructure.State.CMD;
using Infrastructure.State.CMD.Commands.GameResources;
using Infrastructure.State.GameResources;
using R3;
using UnityEngine;

namespace Infrastructure.Roots.GarageScene.EntryPoint
{
	public sealed class GarageEntryPoint : MonoBehaviour
	{
		[SerializeField] private UIGarageRootBinder _uiRootBinderPrefab;
		[SerializeField] private WorldGarageRootBinder _prefabWorldRootBinder;

		private DIContainer _diContainer;
		private DIContainer _viewsDIContainer;

		public Observable<GarageExitParams> Run(DIContainer diContainer, GarageEnterParams enterParams)
		{
			_diContainer = diContainer;
			GarageRegistrations.Register(_diContainer, enterParams);
			_viewsDIContainer = new DIContainer(_diContainer);
			GarageViewModelRegistrations.Register(_viewsDIContainer);

			InitWorld();
			InitUI();

			Debug.Log($"Entering main menu entry point: {enterParams?.DebugData}");

			return CreateExitSignal();
		}
		
		//debug update
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F2))
			{
				_viewsDIContainer.Resolve<CommandProcessor>().ProcessCommand(new CMDResourcesAdd(ResourceType.Soft, 10));
			}
			
			if (Input.GetKeyDown(KeyCode.F3))
			{
				_viewsDIContainer.Resolve<CommandProcessor>().ProcessCommand(new CMDResourcesSpend(ResourceType.Soft, 5));
			}
			
			if (Input.GetKeyDown(KeyCode.F6))
			{
				_viewsDIContainer.Resolve<CommandProcessor>().ProcessCommand(new CMDResourcesAdd(ResourceType.Hard, 5));
			}
			
			if (Input.GetKeyDown(KeyCode.F7))
			{
				_viewsDIContainer.Resolve<CommandProcessor>().ProcessCommand(new CMDResourcesSpend(ResourceType.Hard, 2));
			}
		}

		private Observable<GarageExitParams> CreateExitSignal()
		{
			var mainMenuEnterParams = new MainMenuEnterParams("from garage");
			var gameplayEnterParams = new GameplayEnterParams("from garage");

			var exitToMainMenuSignalSubject = _diContainer.Resolve<Subject<Unit>>(SignalTags.EXIT_TO_MAIN_MENU_SCENE_REQUEST);
			var exitToGameplaySignalSubject = _diContainer.Resolve<Subject<Unit>>(SignalTags.EXIT_TO_GAMEPLAY_SCENE_REQUEST);

			var exitToMainMenuParams = new GarageExitParams(mainMenuEnterParams);
			var exitToGameplayParams = new GarageExitParams(gameplayEnterParams);

			var exitSignal = exitToMainMenuSignalSubject
			                 .Select(_ => exitToMainMenuParams)
			                 .Merge(exitToGameplaySignalSubject.Select(_ => exitToGameplayParams));

			return exitSignal;
		}

		private void InitWorld()
		{
			var assetInstantiateService = _diContainer.Resolve<IAssetInstantiateService>();
			var worldRootBinder = assetInstantiateService.GetInstance(_prefabWorldRootBinder);
			worldRootBinder.Bind(_viewsDIContainer.Resolve<WorldGarageRootViewModel>());
		}

		private void InitUI()
		{
			var uiRoot = _diContainer.Resolve<UIRootView>();
			var uiSceneRootBinder = Instantiate(_uiRootBinderPrefab);
			uiRoot.AttachSceneUI(uiSceneRootBinder.gameObject);

			var uiSceneRootViewModel = _viewsDIContainer.Resolve<UIGarageRootViewModel>();
			uiSceneRootBinder.Bind(uiSceneRootViewModel);

			var uiService = _viewsDIContainer.Resolve<GarageUIService>();
			uiService.OpenMainScreen();
		}
	}
}