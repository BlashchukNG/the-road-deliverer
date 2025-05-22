using Infrastructure.Roots.AppRoot.Services.UserUnput;
using UnityEngine;

namespace Logic.Characters.Base.Locomotions.States
{
	public sealed class InputCalculator
	{
		private readonly IUserInputService _input;
		private readonly CharacterModel _model;

		public InputCalculator(CharacterModel model, IUserInputService input)
		{
			_model = model;
			_input = input;

			_input.onWalkToggled += ToggleWalk;
			_input.onSprintActivated += ActivateSprint;
			_input.onSprintDeactivated += DeactivateSprint;
			_input.onCrouchActivated += ActivateCrouch;
			_input.onCrouchDeactivated += DeactivateCrouch;

			_model.LocomotionRuntimeData.isStrafing = _model.LocomotionSettings.alwaysStrafe;
		}


		public void Calculate(float delta)
		{
			if (_input.MovementInputDetected)
			{
				if (_input.MovementInputDuration == 0)
				{
					_model.LocomotionRuntimeData.movementInputTapped = true;
				}
				else if (_input.MovementInputDuration > 0
				         && _input.MovementInputDuration < _model.LocomotionSettings.buttonHoldThreshold)
				{
					_model.LocomotionRuntimeData.movementInputTapped = false;
					_model.LocomotionRuntimeData.movementInputPressed = true;
					_model.LocomotionRuntimeData.movementInputHeld = false;
				}
				else
				{
					_model.LocomotionRuntimeData.movementInputTapped = false;
					_model.LocomotionRuntimeData.movementInputPressed = false;
					_model.LocomotionRuntimeData.movementInputHeld = true;
				}

				_input.MovementInputDuration += delta;
			}
			else
			{
				_input.MovementInputDuration = 0;
				_model.LocomotionRuntimeData.movementInputTapped = false;
				_model.LocomotionRuntimeData.movementInputPressed = false;
				_model.LocomotionRuntimeData.movementInputHeld = false;
			}

			_model.LocomotionRuntimeData.moveDirection =
				(_model.LocomotionReferences.Camera.GetCameraForwardZeroedYNormalised() * _input.MoveComposite.y)
				+ (_model.LocomotionReferences.Camera.GetCameraRightZeroedYNormalised() * _input.MoveComposite.x);
		}

		public void ActivateSprint()
		{
			if (!_model.LocomotionRuntimeData.isCrouching)
			{
				EnableWalk(false);
				_model.LocomotionRuntimeData.isSprinting = true;
				_model.LocomotionRuntimeData.isStrafing = false;
			}
		}

		private void ToggleWalk()
		{
			EnableWalk(!_model.LocomotionRuntimeData.isWalking);
		}

		private void EnableWalk(bool enable)
		{
			_model.LocomotionRuntimeData.isWalking = enable && _model.LocomotionRuntimeData.isGrounded
			                                                && !_model.LocomotionRuntimeData.isSprinting;
		}

		public void DeactivateSprint()
		{
			_model.LocomotionRuntimeData.isSprinting = false;

			if (_model.LocomotionSettings.alwaysStrafe)
				_model.LocomotionRuntimeData.isStrafing = true;
		}

		public void ActivateCrouch()
		{
			_model.LocomotionRuntimeData.crouchKeyPressed = true;

			if (_model.LocomotionRuntimeData.isGrounded)
			{
				CapsuleCrouchingSize(true);
				DeactivateSprint();
				_model.LocomotionRuntimeData.isCrouching = true;
			}
		}

		public void DeactivateCrouch()
		{
			_model.LocomotionRuntimeData.crouchKeyPressed = false;

			if (!_model.LocomotionRuntimeData.cannotStandUp && !_model.LocomotionRuntimeData.isSliding)
			{
				CapsuleCrouchingSize(false);
				_model.LocomotionRuntimeData.isCrouching = false;
			}
		}

		public void ActivateSliding() => _model.LocomotionRuntimeData.isSliding = true;

		public void DeactivateSliding() => _model.LocomotionRuntimeData.isSliding = false;

		public void CapsuleCrouchingSize(bool crouching)
		{
			if (crouching)
			{
				_model.LocomotionReferences.Controller.center = new Vector3(0f, _model.LocomotionSettings.capsuleCrouchingCentre, 0f);
				_model.LocomotionReferences.Controller.height = _model.LocomotionSettings.capsuleCrouchingHeight;
			}
			else
			{
				_model.LocomotionReferences.Controller.center = new Vector3(0f, _model.LocomotionSettings.capsuleStandingCentre, 0f);
				_model.LocomotionReferences.Controller.height = _model.LocomotionSettings.capsuleStandingHeight;
			}
		}
	}
}