using Infrastructure.DI;
using Infrastructure.Roots.AppRoot.Services.UserUnput;
using Infrastructure.State;
using Logic.Characters.Base;
using Logic.Characters.Base.Animations;
using Logic.Characters.Base.Locomotion;
using Logic.UserCamera;
using UnityEngine;

namespace Logic.Characters.Player
{
	public sealed class PlayerViewModel : BaseCharacterViewModel
	{
		private readonly IUserInputService _input;
		private readonly ICharacterAnimationController _animations;


		public PlayerViewModel(ICharacterView view, DIContainer diContainer) : base(view)
		{
			_input = diContainer.Resolve<IUserInputService>();
			CreateModel(diContainer.Resolve<IGameStateProvider>().GameState.Player.CharacteristicsData);
			var camera = diContainer.Resolve<CameraController>();
			camera.SetFollowTarget(_view.Transform);
			_locomotion = new GroundLocomotion(_view, _model, camera);
			_animations = new CharacterAnimationController(_view, _model, _locomotion);

			_view.onDestroy += Destroy;
			AttachInput();
		}

		private void AttachInput()
		{
			_input.OnMouseButtonDown += MouseButtonDown;
			_input.OnJump += _locomotion.Jump;
			_input.OnJump += _animations.Jump;
			_input.OnRun += _locomotion.Run;
			_input.OnCrouch += _locomotion.Crouch;
		}

		public override void Tick(float delta)
		{
			_locomotion.Move(_input.MousePosition(), _input.HorizontalAxis(), _input.VerticalAxis(), delta);
			_animations.Update(delta);
		}

		public override void FixedTick(float delta)
		{
			_locomotion.CheckGround(delta);
		}

		private void MouseButtonDown(Vector2 position)
		{
		}

		private void Destroy()
		{
			_input.OnMouseButtonDown -= MouseButtonDown;
			_input.OnJump -= _locomotion.Jump;
			_input.OnJump -= _animations.Jump;
			_input.OnRun -= _locomotion.Run;
			_input.OnCrouch -= _locomotion.Crouch;
		}
	}
}