using Infrastructure.DI;
using Infrastructure.Roots.AppRoot;
using Infrastructure.Roots.AppRoot.Services.AssetInstantiate;
using Infrastructure.Roots.GameplayScene.EnterExitParams;
using Infrastructure.Roots.GameplayScene.View;
using Infrastructure.Roots.GarageScene.EnterExitParams;
using Infrastructure.Roots.MainMenuScene.EnterExitParams;
using R3;
using UnityEngine;

namespace Infrastructure.Roots.GameplayScene.EntryPoint
{
	public class GameplayEntryPoint : MonoBehaviour
	{
		[SerializeField] private UIGameplayRootBinder _uiRootBinderPrefab;

		private DIContainer _diContainer;

		public Observable<GameplayExitParams> Run(DIContainer diContainer, GameplayEnterParams enterParams)
		{
			_diContainer = diContainer;
			var sceneUI = _diContainer.Resolve<IAssetInstantiateService>().GetInstance(_uiRootBinderPrefab);
			_diContainer.Resolve<UIRootView>().AttachSceneUI(sceneUI.gameObject);

			var exitToMainMenuSignalSubject = new Subject<Unit>();
			var exitToGarageSignalSubject = new Subject<Unit>();
			
			sceneUI.Bind(exitToMainMenuSignalSubject, exitToGarageSignalSubject);
			
			var mainMenuEnterParams = new MainMenuEnterParams("from gameplay");
			var gameplayEnterParams = new GarageEnterParams("from gameplay");

			var exitToMainMenuParams = new GameplayExitParams(mainMenuEnterParams);
			var exitToGarageParams = new GameplayExitParams(gameplayEnterParams);
			
			var exitSignal = exitToMainMenuSignalSubject
			                 .Select(_ => exitToMainMenuParams)
			                 .Merge(exitToGarageSignalSubject.Select(_ => exitToGarageParams));

			return exitSignal;
		}
	}
}