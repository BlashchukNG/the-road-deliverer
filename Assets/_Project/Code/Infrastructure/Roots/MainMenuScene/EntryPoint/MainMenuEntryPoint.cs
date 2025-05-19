using Infrastructure.DI;
using Infrastructure.Roots.AppRoot;
using Infrastructure.Roots.AppRoot.Services.AssetInstantiate;
using Infrastructure.Roots.GarageScene.EnterExitParams;
using Infrastructure.Roots.MainMenuScene.EnterExitParams;
using Infrastructure.Roots.MainMenuScene.Registrations;
using Infrastructure.Roots.MainMenuScene.View;
using R3;
using UnityEngine;

namespace Infrastructure.Roots.MainMenuScene.EntryPoint
{
	public class MainMenuEntryPoint : MonoBehaviour
	{
		[SerializeField] private UIMainMenuRootBinder _uiRootBinderPrefab;

		private DIContainer _diContainer;
		private DIContainer _viewModelDIContainer;

		public Observable<MainMenuExitParams> Run(DIContainer diContainer, MainMenuEnterParams enterParams)
		{
			_diContainer = diContainer;
			MainMenuRegistrations.Register(_diContainer, enterParams);
			_viewModelDIContainer = new DIContainer(_diContainer);
			MainMenuViewModelRegistrations.Register(_viewModelDIContainer);

			var sceneUI = _diContainer.Resolve<IAssetInstantiateService>().GetInstance(_uiRootBinderPrefab);
			_diContainer.Resolve<UIRootView>().AttachSceneUI(sceneUI.gameObject);

			var exitToGarageSubject = new Subject<Unit>();
			sceneUI.Bind(exitToGarageSubject);

			Debug.Log($"Entering main menu entry point: {enterParams?.DebugData}");

			var exitParams = new MainMenuExitParams(new GarageEnterParams("from main menu"));
			var exitToGarageSignal = exitToGarageSubject.Select(_ => exitParams);

			return exitToGarageSignal;
		}
	}
}