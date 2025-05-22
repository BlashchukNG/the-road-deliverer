using Infrastructure.Roots.AppRoot.Services.UserUnput;
using UnityEngine;

namespace Logic.Characters.Base.Locomotions.States
{
	public sealed class CrouchState : IState
	{
		private readonly StateMachine _stateMachine;
		private readonly IUserInputService _input;
		private readonly CharacterModel _model;
		private readonly Checker _checker;
		private readonly Calculator _calculator;
		private readonly InputCalculator _inputCalculator;
		private readonly AnimatorVariablesUpdater _animatorVariablesUpdater;

		public CrouchState
		(StateMachine stateMachine,
			CharacterModel model,
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

			if (!_model.LocomotionRuntimeData.isGrounded)
			{
				_inputCalculator.DeactivateCrouch();
				_inputCalculator.CapsuleCrouchingSize(false);
				_stateMachine.SwitchState(AnimationState.Fall);
			}

			CeilingHeightCheck();

			if (!_model.LocomotionRuntimeData.crouchKeyPressed && !_model.LocomotionRuntimeData.cannotStandUp)
			{
				_inputCalculator.DeactivateCrouch();
				SwitchToLocomotionState();
			}

			if (!_model.LocomotionRuntimeData.isCrouching)
			{
				_inputCalculator.CapsuleCrouchingSize(false);
				SwitchToLocomotionState();
			}

			_checker.CheckEnableTurns();
			_checker.CheckEnableLean();

			_calculator.CalculateRotationalAdditives(delta, false, _model.LocomotionSettings.enableHeadTurn, false);

			_inputCalculator.Calculate(delta);
			_calculator.CalculateMoveDirection();

			_checker.CheckIfStarting();
			_checker.CheckIfStopped();

			_calculator.CalculateFaceMoveDirection(delta);
			_model.LocomotionReferences.Controller.Move(_model.LocomotionRuntimeData.velocity * delta);
			_animatorVariablesUpdater.UpdateAnimatorController();
		}

		private void CeilingHeightCheck()
		{
			float rayDistance = Mathf.Infinity;
			float minimumStandingHeight = _model.LocomotionSettings.capsuleStandingHeight - _model.LocomotionReferences.FrontRayPos.localPosition.y;

			Vector3 midpoint = new Vector3(_model.LocomotionReferences.Controller.transform.position.x,
				_model.LocomotionReferences.Controller.transform.position.y + _model.LocomotionReferences.FrontRayPos.localPosition.y,
				_model.LocomotionReferences.Controller.transform.position.z);
			if (Physics.Raycast(midpoint, _model.LocomotionReferences.Controller.transform.TransformDirection(Vector3.up),
				    out RaycastHit ceilingHit, rayDistance, _model.LocomotionSettings.groundLayerMask))
				_model.LocomotionRuntimeData.cannotStandUp = ceilingHit.distance < minimumStandingHeight;
			else
				_model.LocomotionRuntimeData.cannotStandUp = false;
		}

		private void SwitchStateToJump()
		{
			if (!_model.LocomotionRuntimeData.cannotStandUp)
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