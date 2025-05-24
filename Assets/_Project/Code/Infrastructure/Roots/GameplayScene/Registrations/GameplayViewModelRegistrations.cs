using Infrastructure.DI;
using Infrastructure.Roots.GameplayScene.Services.UI;
using Infrastructure.Roots.GameplayScene.View;

namespace Infrastructure.Roots.GameplayScene.Registrations
{
	public static class GameplayViewModelRegistrations
	{
		public static void Register(DIContainer diContainer)
		{
			diContainer.RegisterFactory(c => new GameplayUIService(c)).AsSingle();
			diContainer.RegisterFactory(c => new UIGameplayRootViewModel()).AsSingle();
			diContainer.RegisterFactory(c => new WorldGameplayRootViewModel()).AsSingle();
		}
	}
}