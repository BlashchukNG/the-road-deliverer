using Infrastructure.AppRoot;
using Infrastructure.DI;
using Infrastructure.Roots.MainMenuScene.EnterExitParams;
using Infrastructure.Roots.MainMenuScene.UI;
using Infrastructure.Services.AssetInstantiate;
using R3;
using UnityEngine;

namespace Infrastructure.Roots.MainMenuScene.EntryPoint
{
	public class MainMenuEntryPoint : MonoBehaviour
	{
		[SerializeField] private UIMainMenuRootBinder _uiRootBinderPrefab;

		private DIContainer _diContainer;

		public Observable<MainMenuExitParams> Run(DIContainer diContainer, MainMenuEnterParams enterParams)
		{
			_diContainer = diContainer;
			var sceneUI = _diContainer.Resolve<IAssetInstantiateService>().GetInstance(_uiRootBinderPrefab);
			_diContainer.Resolve<UIRootView>().AttachSceneUI(sceneUI.gameObject);

			var exitToGarageSubject = new Subject<Unit>();
			sceneUI.Bind(exitToGarageSubject);

			Debug.Log($"Entering main menu entry point: {enterParams?.DebugData}");
			
			var exitParams = new MainMenuExitParams();
			var exitToGarageSignal = exitToGarageSubject.Select(_ => exitParams);

			return exitToGarageSignal;
		}
	}
}