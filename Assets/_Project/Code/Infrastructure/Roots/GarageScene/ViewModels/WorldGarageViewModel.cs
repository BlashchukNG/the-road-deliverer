using Infrastructure.DI;
using Infrastructure.Roots.AppRoot.Services.Updater;
using Logic.Characters.Player;

namespace Infrastructure.Roots.GarageScene.ViewModels
{
	public sealed class WorldGarageViewModel : ITick
	{
		public DIContainer DIContainer => _diContainer;
		public PlayerViewModel PlayerViewModel => _playerViewModel;

		private readonly DIContainer _diContainer;
		private readonly PlayerViewModel _playerViewModel;


		public WorldGarageViewModel(DIContainer diContainer)
		{
			_diContainer = diContainer;
			_playerViewModel = new PlayerViewModel(_diContainer);
			
			_diContainer.Resolve<IUpdateService>().Add(this);
		}


		public void Tick(float delta)
		{
			_playerViewModel.Tick(delta);
		}
	}
}