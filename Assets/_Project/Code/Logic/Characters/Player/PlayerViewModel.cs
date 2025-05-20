using Infrastructure.DI;
using Infrastructure.Roots.AppRoot.Services.UserUnput;
using Infrastructure.State;
using Logic.Characters.Base;
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
			_locomotion = new GroundLocomotion(_view, _model);

			_view.onDestroy += Destroy;
			AttachInput();
		}

		private void AttachInput()
		{
			_input.OnMouseButtonDown += MouseButtonDown;
			_input.OnJump += _locomotion.Jump;
			_input.OnRun += _locomotion.Run;
			_input.OnCrouch += _locomotion.Crouch;
		}

		public override void Tick(float delta)
		{
			_locomotion.Move(_input.HorizontalAxis(), _input.VerticalAxis(), delta);
		}

		public override void FixedTick(float delta)
		{
		}

		private void MouseButtonDown(Vector2 position)
		{
		}

		private void Destroy()
		{
			_input.OnMouseButtonDown -= MouseButtonDown;
		}
	}
}