using System.Collections;
using Code.Utils.Coroutiner;
using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.AppRoot
{
	public sealed class AddEntryPoint
	{
		private static AddEntryPoint _instance;
		
		private readonly WaitForSeconds _delayBetweenScenes = new(InfrastructureConstants.DELAY_BETWEEN_SCENES);
		private readonly CoroutineRunner _coroutineRunner;
		private readonly UIRootView _uiRootView;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void LoadApp()
		{
			//TODO: add global settings service
			Application.targetFrameRate = 60;
			Screen.sleepTimeout = SleepTimeout.NeverSleep;

			_instance = new AddEntryPoint();
			_instance.RunApp();
		}

		private AddEntryPoint()
		{
			_coroutineRunner = new GameObject("[COROUTINE RUNNER]")
				.AddComponent<CoroutineRunner>();
			Object.DontDestroyOnLoad(_coroutineRunner.gameObject);

			var prefabUIRootView = Resources.Load<UIRootView>("UiRoot");
			_uiRootView = Object.Instantiate(prefabUIRootView);
			Object.DontDestroyOnLoad(_uiRootView.gameObject);

			//TODO: create container
			//TODO: add settings service
		}

		private void RunApp()
		{
		#if UNITY_EDITOR
			var sceneName = SceneManager.GetActiveScene().name;

			switch (sceneName)
			{
				case Scenes.MAIN_MENU:
					//_coroutineRunner.StartCoroutine(LoadMainMenu());
					break;
			}

			if (sceneName != Scenes.BOOT)
				return;
		#endif

			//_coroutineRunner.StartCoroutine(LoadMainMenu());
		}

		// private IEnumerator LoadMainMenu(MainMenuEnterParams enterParams = null)
		// {
		// 	_uiRootView.ShowLoadingScreen();
		//
		// 	yield return LoadScene(Scenes.BOOT);
		// 	yield return LoadScene(Scenes.MAIN_MENU);
		//
		// 	yield return _delayBetweenScenes;
		//
		// 	var isSettingsLoaded = false;
		// 	//load settings
		// 	yield return new WaitUntil(() => isSettingsLoaded);
		//
		// 	var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
		// 	sceneEntryPoint.Run(enterParams)
		// 	               .Subscribe(exitParams =>
		// 	               {
		// 		               var targetSceneName = exitParams.TargetSceneEnterParams.SceneName;
		//
		// 		               // if (targetSceneName == Scenes.GAME_HALL)
		// 		               //  _coroutineRunner.StartCoroutine(LoadGameHall(exitParams.TargetSceneEnterParams.As<GameHallEnterParams>()));
		// 	               });
		//
		// 	_uiRootView.HideLoadingScreen();
		// }

		private IEnumerator LoadScene(string sceneName)
		{
			yield return SceneManager.LoadSceneAsync(sceneName);
		}
	}
}