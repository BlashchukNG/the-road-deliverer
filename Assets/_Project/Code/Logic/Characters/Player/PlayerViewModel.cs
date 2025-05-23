using Infrastructure.DI;
using Infrastructure.Roots.AppRoot.Services.ResourceLoader;
using Infrastructure.Roots.AppRoot.Services.UserUnput;
using Infrastructure.State;
using Logic.Characters.Base;
using Logic.Characters.Base.Locomotions;
using Logic.UserCamera;

namespace Logic.Characters.Player
{
	public sealed class PlayerViewModel : BaseCharacterViewModel
	{
		private readonly IUserInputService _input;
		private readonly CameraController _camera;
		private readonly Locomotion _locomotion;


		public PlayerViewModel(ICharacterView view, DIContainer diContainer) : base(view, diContainer)
		{
			_input = diContainer.Resolve<IUserInputService>();
			_camera = diContainer.Resolve<CameraController>().SetFollowTarget(_view.Transform);

			_view.onDestroy += Destroy;
			CreateModel();

			_locomotion = new Locomotion(_model, _input);
		}

		protected override void CreateModel()
		{
			_model = new CharacterModel(_view, _camera, _input, _diContainer.Resolve<IGameStateProvider>().GameState.Player,
				_diContainer.Resolve<IResourceLoaderService>().GetPlayerLocomotionSettings());
		}

		public override void Tick(float delta)
		{
			_locomotion.Update(delta);
		}

		public override void FixedTick(float delta)
		{
		}

		private void Destroy()
		{
		}
	}
}