using Infrastructure.DI;
using Infrastructure.Roots.AppRoot.Services.Updater;

namespace Infrastructure.Roots.GarageScene.View
{
	public sealed class WorldGarageViewModel : ITick
	{
		private readonly DIContainer _diContainer;

		public WorldGarageViewModel(DIContainer diContainer)
		{
			_diContainer = diContainer;
		}

		public void Tick(float delta)
		{
			
		}
	}
}