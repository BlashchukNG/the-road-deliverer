using Infrastructure.Roots.AppRoot.Services.UserUnput;
using UnityEngine;

namespace Logic.Characters.Base.Locomotions.States
{
	public sealed class JumpState : IState
	{
		private readonly StateMachine _stateMachine;
		private readonly IUserInputService _input;
		private readonly CharacterModel _model;
		private readonly Checker _checker;
		private readonly Calculator _calculator;
		private readonly InputCalculator _inputCalculator;
		private readonly AnimatorVariablesUpdater _animatorVariablesUpdater;

		public JumpState
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
			_model.LocomotionReferences.Animator.SetBool(_model.LocomotionAnimationsVariables.isJumpingAnimHash, false);
		}

		public IState Enter()
		{
			_model.LocomotionReferences.Animator.SetBool(_model.LocomotionAnimationsVariables.isJumpingAnimHash, true);
			_model.LocomotionRuntimeData.isSliding = false;
			_model.LocomotionRuntimeData.velocity = new Vector3(_model.LocomotionRuntimeData.velocity.x,
				_model.LocomotionSettings.jumpForce, _model.LocomotionRuntimeData.velocity.z);

			return this;
		}

		public void Update(float delta)
		{
			ApplyGravity();

			if (_model.LocomotionRuntimeData.velocity.y <= 0f)
			{
				_model.LocomotionReferences.Animator.SetBool(_model.LocomotionAnimationsVariables.isJumpingAnimHash, false);
				_stateMachine.SwitchState(AnimationState.Fall);
			}

			_checker.GroundCheck();

			_calculator.CalculateRotationalAdditives(delta, false, _model.LocomotionSettings.enableHeadTurn,
				_model.LocomotionSettings.enableBodyTurn);
			_calculator.CalculateMoveDirection();
			_calculator.CalculateFaceMoveDirection(delta);
			_model.LocomotionReferences.Controller.Move(_model.LocomotionRuntimeData.velocity * delta);
			_animatorVariablesUpdater.UpdateAnimatorController();
		}

		private void ApplyGravity()
		{
			if (_model.LocomotionRuntimeData.velocity.y > Physics.gravity.y)
				_model.LocomotionRuntimeData.velocity.y += Physics.gravity.y * _model.LocomotionSettings.gravityMultiplier * Time.deltaTime;
		}
	}
}