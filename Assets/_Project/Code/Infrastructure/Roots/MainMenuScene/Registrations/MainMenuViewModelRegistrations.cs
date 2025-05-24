using Infrastructure.DI;
using Infrastructure.Roots.MainMenuScene.Services.UI;
using Infrastructure.Roots.MainMenuScene.Views;

namespace Infrastructure.Roots.MainMenuScene.Registrations
{
	public static class MainMenuViewModelRegistrations
	{
		public static void Register(DIContainer diContainer)
		{
			diContainer.RegisterFactory(c => new MainMenuUIService(c)).AsSingle();
			diContainer.RegisterFactory(_ => new UIMainMenuRootViewModel()).AsSingle();
		}
	}
}