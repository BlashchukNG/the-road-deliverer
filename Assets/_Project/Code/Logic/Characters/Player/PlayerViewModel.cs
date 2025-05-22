using Infrastructure.DI;
using Infrastructure.Roots.AppRoot.Services.UserUnput;
using Infrastructure.State;
using Logic.Characters.Base;
using Logic.Characters.Base.Locomotions;
using Logic.UserCamera;
using UnityEngine;

namespace Logic.Characters.Player
{
	public sealed class PlayerViewModel : BaseCharacterViewModel
	{
		private readonly IUserInputService _input;


		public PlayerViewModel(ICharacterView view, DIContainer diContainer) : base(view)
		{
			_input = diContainer.Resolve<IUserInputService>();
			CreateModel(diContainer.Resolve<IGameStateProvider>().GameState.Player.CharacteristicsData);
			var camera = diContainer.Resolve<CameraController>();
			camera.SetFollowTarget(_view.Transform);
			_locomotion = new Locomotion(view as PlayerView, _input, camera);

			_view.onDestroy += Destroy;
			AttachInput();
		}

		private void AttachInput()
		{
			
		}

		public override void Tick(float delta)
		{
			_locomotion.Tick(delta);
		}

		public override void FixedTick(float delta)
		{
			
		}

		private void MouseButtonDown(Vector2 position)
		{
		}

		private void Destroy()
		{
			
		}
	}
}