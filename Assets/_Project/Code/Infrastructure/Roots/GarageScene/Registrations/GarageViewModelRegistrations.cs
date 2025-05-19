using System;
using Infrastructure.DI;
using Infrastructure.Roots.AppRoot.Services.AssetInstantiate;
using Infrastructure.Roots.AppRoot.Services.Updater;
using Infrastructure.Roots.AppRoot.Services.UserUnput;
using Infrastructure.Roots.GarageScene.View;
using UnityEngine;

namespace Infrastructure.Roots.GarageScene.Registrations
{
	public static class GarageViewModelRegistrations
	{
		public static void Register(DIContainer diContainer)
		{
			var updater = diContainer.Resolve<IAssetInstantiateService>().GetUpdater();
			diContainer.RegisterInstance(updater);

			RegisterInputService(diContainer);

			diContainer.RegisterFactory(c => new UIGarageViewModel()).AsSingle();
			diContainer.RegisterFactory(c => new WorldGarageViewModel()).AsSingle();
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
		}
	}
}