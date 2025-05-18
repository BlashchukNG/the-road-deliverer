using Infrastructure.AppRoot;
using Infrastructure.DI;
using Infrastructure.Roots.GameplayScene.EnterExitParams;
using Infrastructure.Roots.GarageScene.EnterExitParams;
using Infrastructure.Roots.GarageScene.UI;
using Infrastructure.Roots.MainMenuScene.EnterExitParams;
using Infrastructure.Services.AssetInstantiate;
using R3;
using UnityEngine;

namespace Infrastructure.Roots.GarageScene.EntryPoint
{
	public class GarageEntryPoint : MonoBehaviour
	{
		[SerializeField] private UIGarageRootBinder _uiRootBinderPrefab;

		private DIContainer _diContainer;

		public Observable<GarageExitParams> Run(DIContainer diContainer, GarageEnterParams enterParams)
		{
			_diContainer = diContainer;
			var sceneUI = _diContainer.Resolve<IAssetInstantiateService>().GetInstance(_uiRootBinderPrefab);
			_diContainer.Resolve<UIRootView>().AttachSceneUI(sceneUI.gameObject);

			var exitToMainMenuSignalSubject = new Subject<Unit>();
			var exitToGameplaySignalSubject = new Subject<Unit>();
			
			sceneUI.Bind(exitToMainMenuSignalSubject, exitToGameplaySignalSubject);
			
			var mainMenuEnterParams = new MainMenuEnterParams("from garage");
			var gameplayEnterParams = new GameplayEnterParams("from garage");

			var exitToMainMenuParams = new GarageExitParams(mainMenuEnterParams);
			var exitToGameplayParams = new GarageExitParams(gameplayEnterParams);
			
			var exitSignal = exitToMainMenuSignalSubject
			                 .Select(_ => exitToMainMenuParams)
			                 .Merge(exitToGameplaySignalSubject.Select(_ => exitToGameplayParams));

			
			return exitSignal;
		}
	}
}