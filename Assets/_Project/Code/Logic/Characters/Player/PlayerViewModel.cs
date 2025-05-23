using Infrastructure.DI;
using Infrastructure.Roots.AppRoot.Services.ResourceLoader;
using Infrastructure.Roots.AppRoot.Services.UserInput;
using Infrastructure.State;
using Logic.Characters.Base;
using Logic.Characters.Base.Locomotions;
using Logic.UserCamera;

namespace Logic.Characters.Player
{
	public sealed class PlayerViewModel
	{
		private readonly IUserInputService _input;
		private readonly CameraController _camera;
		private readonly Locomotion _locomotion;
		private readonly DIContainer _diContainer;
		private readonly CharacterModel _model;

		public PlayerViewModel(DIContainer diContainer)
		{
			_diContainer = diContainer;
			_input = _diContainer.Resolve<IUserInputService>();
			_camera = _diContainer.Resolve<CameraController>().SetFollowTarget(_view.Transform);
			_model = new CharacterModel(_camera, _input, _diContainer.Resolve<IGameStateProvider>().GameState.Player,
				_diContainer.Resolve<IResourceLoaderService>().GetPlayerLocomotionSettings());

			_locomotion = new Locomotion(_model, _input);
		}

		public override void Tick(float delta)
		{
			_locomotion.Update(delta);
		}

		public override void FixedTick(float delta)
		{
		}

	}
}