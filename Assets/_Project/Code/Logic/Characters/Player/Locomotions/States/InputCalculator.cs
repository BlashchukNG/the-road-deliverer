using Infrastructure.Roots.AppRoot.Services.UserInput;
using UnityEngine;

namespace Logic.Characters.Player.Locomotions.States
{
	public sealed class InputCalculator
	{
		private readonly IUserInputService _input;
		private readonly PlayerBehavior _model;

		public InputCalculator(PlayerBehavior model, IUserInputService input)
		{
			_model = model;
			_input = input;

			_input.onWalkToggled += ToggleWalk;
			_input.onSprintActivated += ActivateSprint;
			_input.onSprintDeactivated += DeactivateSprint;
			_input.onCrouchActivated += ActivateCrouch;
			_input.onCrouchDeactivated += DeactivateCrouch;

			_model.RuntimeData.isStrafing = _model.Settings.alwaysStrafe;
		}


		public void Calculate(float delta)
		{
			if (_input.MovementInputDetected)
			{
				if (_input.MovementInputDuration == 0)
				{
					_model.RuntimeData.movementInputTapped = true;
				}
				else if (_input.MovementInputDuration > 0
				         && _input.MovementInputDuration < _model.Settings.buttonHoldThreshold)
				{
					_model.RuntimeData.movementInputTapped = false;
					_model.RuntimeData.movementInputPressed = true;
					_model.RuntimeData.movementInputHeld = false;
				}
				else
				{
					_model.RuntimeData.movementInputTapped = false;
					_model.RuntimeData.movementInputPressed = false;
					_model.RuntimeData.movementInputHeld = true;
				}

				_input.MovementInputDuration += delta;
			}
			else
			{
				_input.MovementInputDuration = 0;
				_model.RuntimeData.movementInputTapped = false;
				_model.RuntimeData.movementInputPressed = false;
				_model.RuntimeData.movementInputHeld = false;
			}

			_model.RuntimeData.moveDirection =
				(_model.References.Camera.GetCameraForwardZeroedYNormalised() * _input.MoveComposite.y)
				+ (_model.References.Camera.GetCameraRightZeroedYNormalised() * _input.MoveComposite.x);
		}

		public void ActivateSprint()
		{
			if (!_model.RuntimeData.isCrouching)
			{
				EnableWalk(false);
				_model.RuntimeData.isSprinting = true;
				_model.RuntimeData.isStrafing = false;
			}
		}

		private void ToggleWalk()
		{
			EnableWalk(!_model.RuntimeData.isWalking);
		}

		private void EnableWalk(bool enable)
		{
			_model.RuntimeData.isWalking = enable && _model.RuntimeData.isGrounded
			                                                && !_model.RuntimeData.isSprinting;
		}

		public void DeactivateSprint()
		{
			_model.RuntimeData.isSprinting = false;

			if (_model.Settings.alwaysStrafe)
				_model.RuntimeData.isStrafing = true;
		}

		public void ActivateCrouch()
		{
			_model.RuntimeData.crouchKeyPressed = true;

			if (_model.RuntimeData.isGrounded)
			{
				CapsuleCrouchingSize(true);
				DeactivateSprint();
				_model.RuntimeData.isCrouching = true;
			}
		}

		public void DeactivateCrouch()
		{
			_model.RuntimeData.crouchKeyPressed = false;

			if (!_model.RuntimeData.cannotStandUp && !_model.RuntimeData.isSliding)
			{
				CapsuleCrouchingSize(false);
				_model.RuntimeData.isCrouching = false;
			}
		}

		public void ActivateSliding() => _model.RuntimeData.isSliding = true;

		public void DeactivateSliding() => _model.RuntimeData.isSliding = false;

		public void CapsuleCrouchingSize(bool crouching)
		{
			if (crouching)
			{
				_model.References.Controller.center = new Vector3(0f, _model.Settings.capsuleCrouchingCentre, 0f);
				_model.References.Controller.height = _model.Settings.capsuleCrouchingHeight;
			}
			else
			{
				_model.References.Controller.center = new Vector3(0f, _model.Settings.capsuleStandingCentre, 0f);
				_model.References.Controller.height = _model.Settings.capsuleStandingHeight;
			}
		}
	}
}