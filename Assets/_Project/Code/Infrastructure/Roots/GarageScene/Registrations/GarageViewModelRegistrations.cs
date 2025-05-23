using System;
using Infrastructure.DI;
using Infrastructure.Roots.AppRoot.Services.AssetInstantiate;
using Infrastructure.Roots.AppRoot.Services.Updater;
using Infrastructure.Roots.AppRoot.Services.UserInput;
using Infrastructure.Roots.GarageScene.ViewModels;
using Infrastructure.State;
using UnityEngine;

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

			diContainer.RegisterFactory(c => new UIGarageViewModel()).AsSingle();
			diContainer.RegisterFactory(c => new WorldGarageViewModel(diContainer)).AsSingle();
		}

		private static void RegisterInputService(DIContainer diContainer)
		{
			IUserInputService inputService;

			switch (Application.platform)
			{
				case RuntimePlatform.WindowsPlayer:
				case RuntimePlatform.WindowsEditor:
				case RuntimePlatform.WebGLPlayer:
					inputService = new PCUserInputService(diContainer.Resolve<IUpdateService>());
					break;
				case RuntimePlatform.IPhonePlayer:
				case RuntimePlatform.Android:
					inputService = new MobileUserInputService();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			diContainer.RegisterInstance(inputService);

			var gameStateProvider = diContainer.Resolve<IGameStateProvider>();
			inputService.onSaveGame += () => gameStateProvider.SaveGameState();
		}
	}
}