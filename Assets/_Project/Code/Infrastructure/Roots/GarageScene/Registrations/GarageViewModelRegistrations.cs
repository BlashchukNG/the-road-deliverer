using System;
using Infrastructure.DI;
using Infrastructure.Roots.AppRoot;
using Infrastructure.Roots.AppRoot.Services.AssetInstantiate;
using Infrastructure.Roots.AppRoot.Services.ResourceLoader;
using Infrastructure.Roots.AppRoot.Services.Updater;
using Infrastructure.Roots.AppRoot.Services.UserUnput;
using Infrastructure.Roots.GarageScene.View;
using Infrastructure.State;
using Logic.Characters.Player;
using Logic.UserCamera;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Roots.GarageScene.Registrations
{
	public static class GarageViewModelRegistrations
	{
		public static void Register(DIContainer diContainer)
		{
			var assetInstantiateService = diContainer.Resolve<IAssetInstantiateService>();
			var updater = assetInstantiateService.GetUpdater();
			diContainer.RegisterInstance(updater);

			RegisterInputService(diContainer);

			var resourceLoaderService = diContainer.Resolve<IResourceLoaderService>();

			// diContainer.RegisterFactory(c => new UIGarageViewModel()).AsSingle();
			// diContainer.RegisterFactory(c => new WorldGarageViewModel()).AsSingle();

			var sceneUIPrefab = resourceLoaderService.GetPrefab<UIGarageRootBinder>("scene ui garage");
			var sceneUI = assetInstantiateService.GetInstance(sceneUIPrefab);
			diContainer.RegisterInstance(sceneUI);
			diContainer.Resolve<UIRootView>().AttachSceneUI(sceneUI.gameObject);

			var worldViewPrefab = resourceLoaderService.GetPrefab<WorldGarageView>("world garage view");
			var worldView = assetInstantiateService.GetInstance(worldViewPrefab);
			diContainer.RegisterInstance(worldView);

			diContainer.RegisterInstance(Object.FindFirstObjectByType<CameraController>().SetInput(diContainer));
			updater.Add(diContainer.Resolve<CameraController>());

			var playerState = diContainer.Resolve<IGameStateProvider>().GameState.Player;
			var playerPrefab = resourceLoaderService.GetPrefab<PlayerView>("player prefab");
			var player = assetInstantiateService.GetInstance(playerPrefab, worldView.layerPlayer, playerState.Position.Value, Quaternion.Euler(playerState.Rotation.Value));
			var playerViewModel = new PlayerViewModel(player, diContainer);
			updater.Add(playerViewModel);
			diContainer.RegisterInstance(playerViewModel);
		}

		private static void RegisterInputService(DIContainer diContainer)
		{
			IUserInputService inputService;

			switch (Application.platform)
			{
				case RuntimePlatform.WindowsPlayer:
				case RuntimePlatform.WindowsEditor:
				case RuntimePlatform.WebGLPlayer:
					inputService = new PCUserInputService();
					break;
				case RuntimePlatform.IPhonePlayer:
				case RuntimePlatform.Android:
					inputService = new MobileUserInputService();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			diContainer.Resolve<IUpdateService>().Add(inputService);
			diContainer.RegisterInstance(inputService);

			var gameStateProvider = diContainer.Resolve<IGameStateProvider>();
			inputService.onSaveGame += () => gameStateProvider.SaveGameState();
		}
	}
}