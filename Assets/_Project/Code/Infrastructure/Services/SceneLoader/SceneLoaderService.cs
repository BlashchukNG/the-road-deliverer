using System.Collections;
using Constants;
using Infrastructure.AppRoot;
using Infrastructure.DI;
using Infrastructure.Roots.GameplayScene.EnterExitParams;
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

			yield return _delayBetweenScenes;
			yield return LoadScene(Scenes.BOOT);
			yield return LoadScene(Scenes.MAIN_MENU);
			yield return _delayBetweenScenes;

			var isSettingsLoaded = false;

			var mainMenuDiContainer = new DIContainer(_diContainer);

			_diContainer.Resolve<IAssetInstantiateService>().GetCameraController();

			isSettingsLoaded = true;

			yield return new WaitUntil(() => isSettingsLoaded);

			Object.FindFirstObjectByType<MainMenuEntryPoint>()
			      .Run(mainMenuDiContainer, enterParams);

			_uiRootView.HideLoadingScreen();
		}

		#endregion

		#region Garage

		public void LoadGarage(GarageEnterParams enterParams = null) => _coroutineRunner.StartCoroutine(LoadGarageRoutine(enterParams));

		private IEnumerator LoadGarageRoutine(GarageEnterParams enterParams)
		{
			_uiRootView.ShowLoadingScreen();

			yield return _delayBetweenScenes;
			yield return LoadScene(Scenes.BOOT);
			yield return LoadScene(Scenes.GARAGE);
			yield return _delayBetweenScenes;

			var isSettingsLoaded = false;

			var garageDiContainer = new DIContainer(_diContainer);

			_diContainer.Resolve<IAssetInstantiateService>().GetCameraController();

			isSettingsLoaded = true;

			yield return new WaitUntil(() => isSettingsLoaded);

			var sceneEntryPoint = Object.FindFirstObjectByType<GarageEntryPoint>();
			sceneEntryPoint.Run(garageDiContainer, enterParams)
			               .Subscribe(exitParams => { LoadMainMenu(exitParams.MainMenuEnterParams); });

			_uiRootView.HideLoadingScreen();
		}

		#endregion

		#region Gameplay

		public void LoadGameplay(GameplayEnterParams enterParams = null)
		{
		}

		#endregion

		private IEnumerator LoadScene(string sceneName)
		{
			yield return SceneManager.LoadSceneAsync(sceneName);
		}
	}
}