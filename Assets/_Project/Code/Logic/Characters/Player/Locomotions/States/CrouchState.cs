using Infrastructure.Roots.AppRoot.Services.UserInput;
using UnityEngine;

namespace Logic.Characters.Base.Locomotions.States
{
	public sealed class CrouchState : IState
	{
		private readonly StateMachine _stateMachine;
		private readonly IUserInputService _input;
		private readonly PlayerBehavior _model;
		private readonly Checker _checker;
		private readonly Calculator _calculator;
		private readonly InputCalculator _inputCalculator;
		private readonly AnimatorVariablesUpdater _animatorVariablesUpdater;

		public CrouchState
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

			if (!_model.RuntimeData.isGrounded)
			{
				_inputCalculator.DeactivateCrouch();
				_inputCalculator.CapsuleCrouchingSize(false);
				_stateMachine.SwitchState(AnimationState.Fall);
			}

			CeilingHeightCheck();

			if (!_model.RuntimeData.crouchKeyPressed && !_model.RuntimeData.cannotStandUp)
			{
				_inputCalculator.DeactivateCrouch();
				SwitchToLocomotionState();
			}

			if (!_model.RuntimeData.isCrouching)
			{
				_inputCalculator.CapsuleCrouchingSize(false);
				SwitchToLocomotionState();
			}

			_checker.CheckEnableTurns();
			_checker.CheckEnableLean();

			_calculator.CalculateRotationalAdditives(delta, false, _model.Settings.enableHeadTurn, false);

			_inputCalculator.Calculate(delta);
			_calculator.CalculateMoveDirection();

			_checker.CheckIfStarting();
			_checker.CheckIfStopped();

			_calculator.CalculateFaceMoveDirection(delta);
			_model.References.Controller.Move(_model.RuntimeData.velocity * delta);
			_animatorVariablesUpdater.UpdateAnimatorController();
		}

		private void CeilingHeightCheck()
		{
			float rayDistance = Mathf.Infinity;
			float minimumStandingHeight = _model.Settings.capsuleStandingHeight - _model.References.FrontRayPos.localPosition.y;

			Vector3 midpoint = new Vector3(_model.References.Controller.transform.position.x,
				_model.References.Controller.transform.position.y + _model.References.FrontRayPos.localPosition.y,
				_model.References.Controller.transform.position.z);
			if (Physics.Raycast(midpoint, _model.References.Controller.transform.TransformDirection(Vector3.up),
				    out RaycastHit ceilingHit, rayDistance, _model.Settings.groundLayerMask))
				_model.RuntimeData.cannotStandUp = ceilingHit.distance < minimumStandingHeight;
			else
				_model.RuntimeData.cannotStandUp = false;
		}

		private void SwitchStateToJump()
		{
			if (!_model.RuntimeData.cannotStandUp)
			{
				_inputCalculator.DeactivateCrouch();
				_stateMachine.SwitchState(AnimationState.Jump);
			}
		}

		private void SwitchToLocomotionState()
		{
			_inputCalculator.DeactivateCrouch();
			_stateMachine.SwitchState(AnimationState.Locomotion);
		}
	}
}