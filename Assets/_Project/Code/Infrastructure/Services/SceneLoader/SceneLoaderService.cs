using System.Collections;
using Constants;
using Infrastructure.AppRoot;
using Infrastructure.DI;
using Infrastructure.Roots.GameplayScene.EnterExitParams;
using Infrastructure.Roots.GameplayScene.EntryPoint;
using Infrastructure.Roots.GarageScene.EnterExitParams;
using Infrastructure.Roots.GarageScene.EntryPoint;
using Infrastructure.Roots.MainMenuScene.EnterExitParams;
using Infrastructure.Roots.MainMenuScene.EntryPoint;
using Infrastructure.Services.AssetInstantiate;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Coroutiner;

namespace Infrastructure.Services.SceneLoader
{
	public class SceneLoaderService : ISceneLoaderService
	{
		private readonly WaitForSeconds _delayBetweenScenes = new(InfrastructureConstants.DELAY_BETWEEN_SCENES);
		private readonly WaitForSeconds _delayBeforeLoadScene = new(InfrastructureConstants.SHOW_HIDE_LOADING_SCREEN_DURATION);

		private readonly DIContainer _diContainer;
		private readonly UIRootView _uiRootView;
		private readonly CoroutineRunner _coroutineRunner;

		public SceneLoaderService(DIContainer diContainer)
		{
			_diContainer = diContainer;
			_uiRootView = _diContainer.Resolve<UIRootView>();
			_coroutineRunner = _diContainer.Resolve<CoroutineRunner>();
		}

		#region MainMenu

		public void LoadMainMenu(MainMenuEnterParams enterParams = null) => _coroutineRunner.StartCoroutine(LoadMainMenuRoutine(enterParams));

		private IEnumerator LoadMainMenuRoutine(MainMenuEnterParams enterParams)
		{
			_uiRootView.ShowLoadingScreen();

			yield return _delayBeforeLoadScene;
			yield return LoadScene(Scenes.BOOT);
			yield return LoadScene(Scenes.MAIN_MENU);
			yield return _delayBetweenScenes;

			var isSettingsLoaded = false;

			var mainMenuDiContainer = new DIContainer(_diContainer);

			var camera = _diContainer.Resolve<IAssetInstantiateService>().GetCameraController();
			mainMenuDiContainer.RegisterInstance(camera);

			isSettingsLoaded = true;

			yield return new WaitUntil(() => isSettingsLoaded);

			var entryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
			entryPoint.Run(mainMenuDiContainer, enterParams)
			          .Subscribe(exitParams => { LoadGarage(exitParams.GarageExitParams); });

			_uiRootView.HideLoadingScreen();
		}

		#endregion

		#region Garage

		public void LoadGarage(GarageEnterParams enterParams = null) => _coroutineRunner.StartCoroutine(LoadGarageRoutine(enterParams));

		private IEnumerator LoadGarageRoutine(GarageEnterParams enterParams)
		{
			_uiRootView.ShowLoadingScreen();

			yield return _delayBeforeLoadScene;
			yield return LoadScene(Scenes.BOOT);
			yield return LoadScene(Scenes.GARAGE);
			yield return _delayBetweenScenes;

			var isSettingsLoaded = false;

			var garageDiContainer = new DIContainer(_diContainer);

			var camera = _diContainer.Resolve<IAssetInstantiateService>().GetCameraController();
			garageDiContainer.RegisterInstance(camera);

			isSettingsLoaded = true;

			yield return new WaitUntil(() => isSettingsLoaded);

			var sceneEntryPoint = Object.FindFirstObjectByType<GarageEntryPoint>();
			sceneEntryPoint.Run(garageDiContainer, enterParams)
			               .Subscribe(exitParams =>
			               {
				               switch (exitParams.TargetSceneEnterParams.SceneName)
				               {
					               case Scenes.MAIN_MENU:
						               LoadMainMenu(exitParams.TargetSceneEnterParams.As<MainMenuEnterParams>());
						               break;

					               case Scenes.GAMEPLAY:
						               LoadGameplay(exitParams.TargetSceneEnterParams.As<GameplayEnterParams>());
						               break;
				               }
			               });

			_uiRootView.HideLoadingScreen();
		}

		#endregion

		#region Gameplay

		public void LoadGameplay(GameplayEnterParams enterParams = null) => _coroutineRunner.StartCoroutine(LoadGameplayRoutine(enterParams));
		
		private IEnumerator LoadGameplayRoutine(GameplayEnterParams enterParams)
		{
			_uiRootView.ShowLoadingScreen();

			yield return _delayBeforeLoadScene;
			yield return LoadScene(Scenes.BOOT);
			yield return LoadScene(Scenes.GAMEPLAY);
			yield return _delayBetweenScenes;

			var isSettingsLoaded = false;

			var gameplayDiContainer = new DIContainer(_diContainer);

			var camera = _diContainer.Resolve<IAssetInstantiateService>().GetCameraController();
			gameplayDiContainer.RegisterInstance(camera);

			isSettingsLoaded = true;

			yield return new WaitUntil(() => isSettingsLoaded);

			var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
			sceneEntryPoint.Run(gameplayDiContainer, enterParams)
			               .Subscribe(exitParams =>
			               {
				               switch (exitParams.TargetSceneEnterParams.SceneName)
				               {
					               case Scenes.MAIN_MENU:
						               LoadMainMenu(exitParams.TargetSceneEnterParams.As<MainMenuEnterParams>());
						               break;

					               case Scenes.GARAGE:
						               LoadGarage(exitParams.TargetSceneEnterParams.As<GarageEnterParams>());
						               break;
				               }
			               });

			_uiRootView.HideLoadingScreen();
		}

		#endregion

		private IEnumerator LoadScene(string sceneName)
		{
			yield return SceneManager.LoadSceneAsync(sceneName);
		}
	}
}