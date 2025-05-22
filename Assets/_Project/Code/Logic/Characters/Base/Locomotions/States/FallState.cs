using Infrastructure.Roots.AppRoot.Services.UserUnput;
using UnityEngine;

namespace Logic.Characters.Base.Locomotions.States
{
	public sealed class FallState : IState
	{
		private readonly StateMachine _stateMachine;
		private readonly IUserInputService _input;
		private readonly CharacterModel _model;
		private readonly Checker _checker;
		private readonly Calculator _calculator;
		private readonly InputCalculator _inputCalculator;
		private readonly AnimatorVariablesUpdater _animatorVariablesUpdater;

		public FallState
		(StateMachine stateMachine,
			CharacterModel model,
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
			_model.LocomotionRuntimeData.velocity.y = 0f;

			_inputCalculator.DeactivateCrouch();
			_model.LocomotionRuntimeData.isSliding = false;

			return this;
		}

		public void Update(float delta)
		{
			_checker.GroundCheck();

			_calculator.CalculateRotationalAdditives(delta, false, _model.LocomotionSettings.enableHeadTurn,
				_model.LocomotionSettings.enableBodyTurn);

			_calculator.CalculateMoveDirection();
			_calculator.CalculateFaceMoveDirection(delta);

			ApplyGravity();

			_model.LocomotionReferences.Controller.Move(_model.LocomotionRuntimeData.velocity * delta);
			_animatorVariablesUpdater.UpdateAnimatorController();

			if (_model.LocomotionReferences.Controller.isGrounded)
				_stateMachine.SwitchState(AnimationState.Locomotion);
		}

		private void ApplyGravity()
		{
			if (_model.LocomotionRuntimeData.velocity.y > Physics.gravity.y)
				_model.LocomotionRuntimeData.velocity.y += Physics.gravity.y * _model.LocomotionSettings.gravityMultiplier * Time.deltaTime;
		}

		private void ResetFallingDuration()
		{
			_model.LocomotionRuntimeData.fallStartTime = Time.time;
			_model.LocomotionRuntimeData.fallingDuration = 0f;
		}

		private void UpdateFallingDuration()
		{
			_model.LocomotionRuntimeData.fallingDuration = Time.time - _model.LocomotionRuntimeData.fallStartTime;
		}
	}
}