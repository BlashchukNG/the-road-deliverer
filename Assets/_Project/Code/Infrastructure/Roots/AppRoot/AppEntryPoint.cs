using Constants;
using Infrastructure.DI;
using Infrastructure.Roots.AppRoot.Services.AssetInstantiate;
using Infrastructure.Roots.AppRoot.Services.SceneLoader;
using Infrastructure.State;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Coroutiner;

namespace Infrastructure.Roots.AppRoot
{
	public sealed class AppEntryPoint
	{
		private static AppEntryPoint _instance;

		private const string PATH_UI_ROOT = "root/ui root view";

		private readonly DIContainer _diContainer = new();

		private UIRootView _uiRootView;
		private CoroutineRunner _coroutineRunner;
		private IAssetInstantiateService _assetInstantiateService;
		private ISettingsProvider _settingsProvider;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void LoadApp()
		{
			//TODO: add global settings service
			Application.targetFrameRate = 60;
			Screen.sleepTimeout = SleepTimeout.NeverSleep;

			_instance = new AppEntryPoint();
			_instance.RunApp();
		}

		private AppEntryPoint()
		{
			InitServices();
		}

		private async void RunApp()
		{
			await _settingsProvider.LoadGameSettingsAsync();
			
			var sceneLoader = _diContainer.Resolve<ISceneLoaderService>();
		#if UNITY_EDITOR
			var sceneName = SceneManager.GetActiveScene().name;

			switch (sceneName)
			{
				case Scenes.MAIN_MENU:
					sceneLoader.LoadMainMenu();
					break;
				case Scenes.GARAGE:
					sceneLoader.LoadGarage();
					break;
				case Scenes.GAMEPLAY:
					sceneLoader.LoadGameplay();
					break;
			}

			if (sceneName != Scenes.BOOT)
				return;
		#endif

			sceneLoader.LoadMainMenu();
		}

		private void InitServices()
		{
			_settingsProvider = new SettingsProvider();
			_diContainer.RegisterInstance(_settingsProvider);
			
			_assetInstantiateService = new AssetInstantiateService(_diContainer);
			_diContainer.RegisterInstance(_assetInstantiateService);

			_coroutineRunner = _assetInstantiateService.GetCoroutineRunner();
			_diContainer.RegisterInstance(_coroutineRunner);

			_uiRootView = _assetInstantiateService.GetInstance(Resources.Load<UIRootView>(PATH_UI_ROOT), bisDontDestroyOnLoad: true);
			_diContainer.RegisterInstance(_uiRootView);

			_diContainer.RegisterInstance<ISceneLoaderService>(new SceneLoaderService(_diContainer));
			_diContainer.RegisterInstance<IGameStateProvider>(new PlayerPrefsGameStateProvider(_diContainer));
		}
	}
}