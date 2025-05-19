using Infrastructure.DI;
using Infrastructure.Roots.AppRoot.Services.AssetInstantiate;
using Infrastructure.Roots.MainMenuScene.View;

namespace Infrastructure.Roots.MainMenuScene.Registrations
{
	public static class MainMenuViewModelRegistrations
	{
		public static void Register(DIContainer diContainer)
		{
			diContainer.RegisterFactory(c => new UIMainMenuRootViewModel()).AsSingle();
		}
	}
}