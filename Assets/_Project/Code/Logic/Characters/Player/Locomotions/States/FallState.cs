using Infrastructure.Roots.AppRoot.Services.UserInput;
using UnityEngine;

namespace Logic.Characters.Player.Locomotions.States
{
	public sealed class FallState : IState
	{
		private readonly StateMachine _stateMachine;
		private readonly IUserInputService _input;
		private readonly PlayerBehavior _model;
		private readonly Checker _checker;
		private readonly Calculator _calculator;
		private readonly InputCalculator _inputCalculator;
		private readonly AnimatorVariablesUpdater _animatorVariablesUpdater;

		public FallState
		(StateMachine stateMachine,
			PlayerBehavior model,
			Checker checker, Calculator calculator,
			InputCalculator inputCalculator,
			AnimatorVariablesUpdater animatorVariablesUpdater)
		{
			_stateMachine = stateMachine;
			_model = model;
			_checker = checker;
			_calculator = calculator;
			_inputCalculator = inputCalculator;
			_animatorVariablesUpdater = animatorVariablesUpdater;
		}

		public void Exit()
		{
		}

		public IState Enter()
		{
			ResetFallingDuration();
			_model.RuntimeData.velocity.y = 0f;

			_inputCalculator.DeactivateCrouch();
			_model.RuntimeData.isSliding = false;

			return this;
		}

		public void Update(float delta)
		{
			_checker.GroundCheck();

			_calculator.CalculateRotationalAdditives(delta, false, _model.Settings.enableHeadTurn,
				_model.Settings.enableBodyTurn);

			_calculator.CalculateMoveDirection();
			_calculator.CalculateFaceMoveDirection(delta);

			ApplyGravity();

			_model.References.Controller.Move(_model.RuntimeData.velocity * delta);
			_animatorVariablesUpdater.UpdateAnimatorController();

			if (_model.References.Controller.isGrounded)
				_stateMachine.SwitchState(AnimationState.Locomotion);
		}

		private void ApplyGravity()
		{
			if (_model.RuntimeData.velocity.y > Physics.gravity.y)
				_model.RuntimeData.velocity.y += Physics.gravity.y * _model.Settings.gravityMultiplier * Time.deltaTime;
		}

		private void ResetFallingDuration()
		{
			_model.RuntimeData.fallStartTime = Time.time;
			_model.RuntimeData.fallingDuration = 0f;
		}

		private void UpdateFallingDuration()
		{
			_model.RuntimeData.fallingDuration = Time.time - _model.RuntimeData.fallStartTime;
		}
	}
}