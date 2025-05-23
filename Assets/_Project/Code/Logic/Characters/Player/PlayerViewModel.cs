using Infrastructure.DI;
using Infrastructure.Roots.AppRoot.Services.ResourceLoader;
using Infrastructure.Roots.AppRoot.Services.Updater;
using Infrastructure.Roots.AppRoot.Services.UserInput;
using Infrastructure.State;
using Logic.Characters.Player.Locomotions;
using Logic.UserCamera;

namespace Logic.Characters.Player
{
	public sealed class PlayerViewModel : ITick
	{
		private readonly IUserInputService _input;
		private readonly Locomotion _locomotion;
		private readonly DIContainer _diContainer;
		private readonly PlayerBehavior _behaviour;

		public PlayerViewModel(DIContainer diContainer)
		{
			_diContainer = diContainer;
			_input = _diContainer.Resolve<IUserInputService>();
			_behaviour = new PlayerBehavior(_input, _diContainer.Resolve<IGameStateProvider>().GameState.Player,
				_diContainer.Resolve<IResourceLoaderService>().GetPlayerLocomotionSettings());

			_locomotion = new Locomotion(_behaviour, _input);
		}

		public void Tick(float delta)
		{
			_locomotion.Update(delta);
		}

		public void AttachReferences(PlayerBinder playerBinder, CameraController camera)
		{
			_behaviour.AttachReferences(playerBinder, camera);
		}
	}
}