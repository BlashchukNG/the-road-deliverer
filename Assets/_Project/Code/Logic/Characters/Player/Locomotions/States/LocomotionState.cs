using Infrastructure.Roots.AppRoot.Services.UserInput;

namespace Logic.Characters.Player.Locomotions.States
{
	public sealed class LocomotionState : IState
	{
		private readonly StateMachine _stateMachine;
		private readonly IUserInputService _input;
		private readonly PlayerBehavior _model;
		private readonly Checker _checker;
		private readonly Calculator _calculator;
		private readonly InputCalculator _inputCalculator;
		private readonly AnimatorVariablesUpdater _animatorVariablesUpdater;

		public LocomotionState
		(StateMachine stateMachine,
			PlayerBehavior model,
			IUserInputService input,
			Checker checker, Calculator calculator,
			InputCalculator inputCalculator, 
			AnimatorVariablesUpdater animatorVariablesUpdater)
		{
			_stateMachine = stateMachine;
			_model = model;
			_input = input;
			_checker = checker;
			_calculator = calculator;
			_inputCalculator = inputCalculator;
			_animatorVariablesUpdater = animatorVariablesUpdater;
		}

		public void Exit()
		{
			_input.onJumpPerformed -= SwitchStateToJump;
		}

		public IState Enter()
		{
			_input.onJumpPerformed += SwitchStateToJump;
			return this;
		}

		public void Update(float delta)
		{
			_checker.GroundCheck();

			if (!_model.RuntimeData.isGrounded) _stateMachine.SwitchState(AnimationState.Fall);
			if (_model.RuntimeData.isCrouching) _stateMachine.SwitchState(AnimationState.Crouch);

			_checker.CheckEnableTurns();
			_checker.CheckEnableLean();
			_calculator.CalculateRotationalAdditives(delta, _model.Settings.enableLean, _model.Settings.enableHeadTurn,
				_model.Settings.enableBodyTurn);

			_inputCalculator.Calculate(delta);
			_calculator.CalculateMoveDirection();

			_checker.CheckIfStarting();
			_checker.CheckIfStopped();
			
			_calculator.CalculateFaceMoveDirection(delta);
			_model.References.Controller.Move(_model.RuntimeData.velocity * delta);
			_animatorVariablesUpdater.UpdateAnimatorController();
		}

		private void SwitchStateToJump() => _stateMachine.SwitchState(AnimationState.Jump);
	}
}