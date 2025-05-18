using System.Collections;
using Code.Utils.Coroutiner;
using Constants;
using Infrastructure.AppRoot;
using Infrastructure.DI;
using UnityEngine;
using UnityEngine.SceneManagement;

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

		public void LoadMainMenu()
		{
			_coroutineRunner.StartCoroutine(LoadMainMenuRoutine());
		}

		private IEnumerator LoadMainMenuRoutine()
		{
			_uiRootView.ShowLoadingScreen();

			yield return _delayBetweenScenes;
			yield return LoadScene(Scenes.BOOT);
			yield return LoadScene(Scenes.MAIN_MENU);
			yield return _delayBetweenScenes;

			var isSettingsLoaded = false;
			//load settings
			
			yield return new WaitUntil(() => isSettingsLoaded);

			// var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
			// sceneEntryPoint.Run(enterParams)
			//                .Subscribe(exitParams =>
			//                {
			// 	               var targetSceneName = exitParams.TargetSceneEnterParams.SceneName;
			//
			// 	               // if (targetSceneName == Scenes.GAME_HALL)
			// 	               //  _coroutineRunner.StartCoroutine(LoadGameHall(exitParams.TargetSceneEnterParams.As<GameHallEnterParams>()));
			//                });

			_uiRootView.HideLoadingScreen();
		}

		private IEnumerator LoadScene(string sceneName)
		{
			yield return SceneManager.LoadSceneAsync(sceneName);
		}
	}
}