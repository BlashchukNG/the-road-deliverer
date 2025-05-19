using Infrastructure.DI;
using Infrastructure.Roots.GarageScene.View;

namespace Infrastructure.Roots.GarageScene.Registrations
{
	public static class GarageViewModelRegistrations
	{
		public static void Register(DIContainer diContainer)
		{
			diContainer.RegisterFactory(c => new UIGarageViewModel()).AsSingle();
			diContainer.RegisterFactory(c => new WorldGarageViewModel()).AsSingle();
		}
	}
}