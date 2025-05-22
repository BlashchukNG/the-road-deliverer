using UnityEngine;

namespace Logic.Characters.Base.Locomotions.States
{
	public sealed class Calculator
	{
		private readonly CharacterModel _model;

		public Calculator(CharacterModel model)
		{
			_model = model;
		}

		public void CalculateRotationalAdditives(float delta, bool leansActivated, bool headLookActivated, bool bodyLookActivated)
		{
			if (headLookActivated || leansActivated || bodyLookActivated)
			{
				_model.LocomotionRuntimeData.currentRotation = _model.LocomotionReferences.Controller.transform.forward;

				_model.LocomotionRuntimeData.rotationRate =
					_model.LocomotionRuntimeData.currentRotation != _model.LocomotionRuntimeData.previousRotation
						? Vector3.SignedAngle(_model.LocomotionRuntimeData.currentRotation, _model.LocomotionRuntimeData.previousRotation, Vector3.up) / delta * -1f
						: 0f;
			}

			_model.LocomotionRuntimeData.initialLeanValue = leansActivated ? _model.LocomotionRuntimeData.rotationRate : 0f;

			var leanSmoothness = 5;
			var maxLeanRotationRate = 275.0f;

			var referenceValue = _model.LocomotionRuntimeData.speed2D / _model.Characteristics.SprintSpeed;
			_model.LocomotionRuntimeData.leanValue = CalculateSmoothedValue(
				_model.LocomotionRuntimeData.leanValue,
				_model.LocomotionRuntimeData.initialLeanValue,
				maxLeanRotationRate,
				leanSmoothness,
				_model.LocomotionSettings.leanCurve,
				referenceValue,
				true
			);

			var headTurnSmoothness = 5f;

			if (headLookActivated && _model.LocomotionRuntimeData.isTurningInPlace)
			{
				_model.LocomotionRuntimeData.initialTurnValue = _model.LocomotionSettings.cameraRotationOffset;
				_model.LocomotionRuntimeData.headLookX = Mathf.Lerp(_model.LocomotionRuntimeData.headLookX,
					_model.LocomotionRuntimeData.initialTurnValue / 200, 5f * Time.deltaTime);
			}
			else
			{
				_model.LocomotionRuntimeData.initialTurnValue = headLookActivated ? _model.LocomotionRuntimeData.rotationRate : 0f;
				_model.LocomotionRuntimeData.headLookX = CalculateSmoothedValue(
					_model.LocomotionRuntimeData.headLookX,
					_model.LocomotionRuntimeData.initialTurnValue,
					maxLeanRotationRate,
					headTurnSmoothness,
					_model.LocomotionSettings.headLookXCurve,
					_model.LocomotionRuntimeData.headLookX,
					false
				);
			}

			var bodyTurnSmoothness = 5f;

			_model.LocomotionRuntimeData.initialTurnValue = bodyLookActivated ? _model.LocomotionRuntimeData.rotationRate : 0f;

			_model.LocomotionRuntimeData.bodyLookX = CalculateSmoothedValue(
				_model.LocomotionRuntimeData.bodyLookX,
				_model.LocomotionRuntimeData.initialTurnValue,
				maxLeanRotationRate,
				bodyTurnSmoothness,
				_model.LocomotionSettings.bodyLookXCurve,
				_model.LocomotionRuntimeData.bodyLookX,
				false
			);

			var cameraTilt = _model.LocomotionReferences.Camera.GetCameraTiltX();
			cameraTilt = (cameraTilt > 180f ? cameraTilt - 360f : cameraTilt) / -180;
			cameraTilt = Mathf.Clamp(cameraTilt, -0.1f, 1.0f);
			_model.LocomotionRuntimeData.headLookY = cameraTilt;
			_model.LocomotionRuntimeData.bodyLookY = cameraTilt;

			_model.LocomotionRuntimeData.previousRotation = _model.LocomotionRuntimeData.currentRotation;
		}

		private float CalculateSmoothedValue(
			float mainVariable,
			float newValue,
			float maxRateChange,
			float smoothness,
			AnimationCurve referenceCurve,
			float referenceValue,
			bool isMultiplier
		)
		{
			var changeVariable = newValue / maxRateChange;
			changeVariable = Mathf.Clamp(changeVariable, -1.0f, 1.0f);

			if (isMultiplier)
			{
				var multiplier = referenceCurve.Evaluate(referenceValue);
				changeVariable *= multiplier;
			}
			else
				changeVariable = referenceCurve.Evaluate(changeVariable);

			if (!changeVariable.Equals(mainVariable)) changeVariable = Mathf.Lerp(mainVariable, changeVariable, smoothness * Time.deltaTime);

			return changeVariable;
		}

		public void CalculateMoveDirection()
		{
			if (!_model.LocomotionRuntimeData.isGrounded)
				_model.LocomotionRuntimeData.targetMaxSpeed = _model.LocomotionRuntimeData.currentMaxSpeed;
			else if (_model.LocomotionRuntimeData.isCrouching)
				_model.LocomotionRuntimeData.targetMaxSpeed = _model.Characteristics.WalkSpeed;
			else if (_model.LocomotionRuntimeData.isSprinting)
				_model.LocomotionRuntimeData.targetMaxSpeed = _model.Characteristics.SprintSpeed;
			else if (_model.LocomotionRuntimeData.isWalking)
				_model.LocomotionRuntimeData.targetMaxSpeed = _model.Characteristics.WalkSpeed;
			else
				_model.LocomotionRuntimeData.targetMaxSpeed = _model.Characteristics.RunSpeed;

			_model.LocomotionRuntimeData.currentMaxSpeed =
				Mathf.Lerp(_model.LocomotionRuntimeData.currentMaxSpeed, _model.LocomotionRuntimeData.targetMaxSpeed,
					_model.LocomotionSettings.ANIMATION_DAMP_TIME * Time.deltaTime);

			_model.LocomotionRuntimeData.targetVelocity.x = _model.LocomotionRuntimeData.moveDirection.x * _model.LocomotionRuntimeData.currentMaxSpeed;
			_model.LocomotionRuntimeData.targetVelocity.z = _model.LocomotionRuntimeData.moveDirection.z * _model.LocomotionRuntimeData.currentMaxSpeed;

			_model.LocomotionRuntimeData.velocity.z =
				Mathf.Lerp(_model.LocomotionRuntimeData.velocity.z, _model.LocomotionRuntimeData.targetVelocity.z,
					_model.LocomotionSettings.speedChangeDamping * Time.deltaTime);

			_model.LocomotionRuntimeData.velocity.x =
				Mathf.Lerp(_model.LocomotionRuntimeData.velocity.x, _model.LocomotionRuntimeData.targetVelocity.x,
					_model.LocomotionSettings.speedChangeDamping * Time.deltaTime);

			_model.LocomotionRuntimeData.speed2D = new Vector3(_model.LocomotionRuntimeData.velocity.x, 0f, _model.LocomotionRuntimeData.velocity.z).magnitude;
			_model.LocomotionRuntimeData.speed2D = Mathf.Round(_model.LocomotionRuntimeData.speed2D * 1000f) / 1000f;

			var playerForwardVector = _model.LocomotionReferences.Controller.transform.forward;

			_model.LocomotionRuntimeData.newDirectionDifferenceAngle =
				playerForwardVector != _model.LocomotionRuntimeData.moveDirection
					? Vector3.SignedAngle(playerForwardVector, _model.LocomotionRuntimeData.moveDirection, Vector3.up)
					: 0f;

			CalculateGait();
		}
		
		private void CalculateGait()
		{
			var runThreshold = (_model.Characteristics.WalkSpeed + _model.Characteristics.RunSpeed) / 2;
			var sprintThreshold = (_model.Characteristics.RunSpeed + _model.Characteristics.SprintSpeed) / 2;

			if (_model.LocomotionRuntimeData.speed2D < 0.01)
				_model.LocomotionRuntimeData.currentGait = GaitState.Idle;
			else if (_model.LocomotionRuntimeData.speed2D < runThreshold)
				_model.LocomotionRuntimeData.currentGait = GaitState.Walk;
			else if (_model.LocomotionRuntimeData.speed2D < sprintThreshold)
				_model.LocomotionRuntimeData.currentGait = GaitState.Run;
			else
				_model.LocomotionRuntimeData.currentGait = GaitState.Sprint;
		}
		
		public void CalculateFaceMoveDirection(float delta)
		{
			var characterForward = new Vector3(_model.LocomotionReferences.Controller.transform.forward.x, 0f, 
				_model.LocomotionReferences.Controller.transform.forward.z).normalized;
			var characterRight = new Vector3(_model.LocomotionReferences.Controller.transform.right.x, 0f,
				_model.LocomotionReferences.Controller.transform.right.z).normalized;
			var directionForward = new Vector3(_model.LocomotionRuntimeData.moveDirection.x, 0f, _model.LocomotionRuntimeData.moveDirection.z).normalized;

			_model.LocomotionRuntimeData.cameraForward = _model.LocomotionReferences.Camera.GetCameraForwardZeroedYNormalised();
			var strafingTargetRotation = Quaternion.LookRotation(_model.LocomotionRuntimeData.cameraForward);

			_model.LocomotionRuntimeData.strafeAngle = characterForward != directionForward ? Vector3.SignedAngle(characterForward, directionForward, Vector3.up) : 0f;

			_model.LocomotionRuntimeData.isTurningInPlace = false;

			if (_model.LocomotionRuntimeData.isStrafing)
			{
				if (_model.LocomotionRuntimeData.moveDirection.magnitude > 0.01)
				{
					if (_model.LocomotionRuntimeData.cameraForward != Vector3.zero)
					{
						_model.LocomotionSettings.shuffleDirectionZ = Vector3.Dot(characterForward, directionForward);
						_model.LocomotionSettings.shuffleDirectionX = Vector3.Dot(characterRight, directionForward);

						UpdateStrafeDirection(
							Vector3.Dot(characterForward, directionForward),
							Vector3.Dot(characterRight, directionForward)
						);
						_model.LocomotionSettings.cameraRotationOffset = Mathf.Lerp(_model.LocomotionSettings.cameraRotationOffset, 0f,
							_model.LocomotionSettings.rotationSmoothing * delta);

						var targetValue = _model.LocomotionRuntimeData.strafeAngle > _model.LocomotionSettings.forwardStrafeMinThreshold 
						                  && _model.LocomotionRuntimeData.strafeAngle < _model.LocomotionSettings.forwardStrafeMaxThreshold ? 1f : 0f;

						if (Mathf.Abs(_model.LocomotionSettings.forwardStrafe - targetValue) <= 0.001f)
							_model.LocomotionSettings.forwardStrafe = targetValue;
						else
						{
							float t = Mathf.Clamp01(_model.LocomotionSettings.STRAFE_DIRECTION_DAMP_TIME * delta);
							_model.LocomotionSettings.forwardStrafe = Mathf.SmoothStep(_model.LocomotionSettings.forwardStrafe, targetValue, t);
						}
					}

					_model.LocomotionReferences.Controller.transform.rotation = Quaternion.Slerp(_model.LocomotionReferences.Controller.transform.rotation, 
						strafingTargetRotation, _model.LocomotionSettings.rotationSmoothing * delta);
				}
				else
				{
					UpdateStrafeDirection(1f, 0f);

					var t = 20 * delta;
					var newOffset = 0f;

					if (characterForward != _model.LocomotionRuntimeData.cameraForward) newOffset = Vector3.SignedAngle(characterForward, 
						_model.LocomotionRuntimeData.cameraForward, Vector3.up);

					_model.LocomotionSettings.cameraRotationOffset = Mathf.Lerp(_model.LocomotionSettings.cameraRotationOffset, newOffset, t);

					if (Mathf.Abs(_model.LocomotionSettings.cameraRotationOffset) > 10)
					{
						_model.LocomotionRuntimeData.isTurningInPlace = true;
					}
				}
			}
			else
			{
				UpdateStrafeDirection(1f, 0f);
				_model.LocomotionSettings.cameraRotationOffset = Mathf.Lerp(_model.LocomotionSettings.cameraRotationOffset, 0f, 
					_model.LocomotionSettings.rotationSmoothing * delta);

				_model.LocomotionSettings.shuffleDirectionZ = 1;
				_model.LocomotionSettings.shuffleDirectionX = 0;

				var faceDirection = new Vector3(_model.LocomotionRuntimeData.velocity.x, 0f, _model.LocomotionRuntimeData.velocity.z);

				if (faceDirection == Vector3.zero)
					return;

				_model.LocomotionReferences.Controller.transform.rotation = Quaternion.Slerp(
					_model.LocomotionReferences.Controller.transform.rotation,
					Quaternion.LookRotation(faceDirection),
					_model.LocomotionSettings.rotationSmoothing * Time.deltaTime
				);
			}
		}
		
		private void UpdateStrafeDirection(float TargetZ, float TargetX)
		{
			_model.LocomotionRuntimeData.strafeDirectionZ = Mathf.Lerp(_model.LocomotionRuntimeData.strafeDirectionZ, TargetZ,
				_model.LocomotionSettings.ANIMATION_DAMP_TIME * Time.deltaTime);
			_model.LocomotionRuntimeData.strafeDirectionX = Mathf.Lerp(_model.LocomotionRuntimeData.strafeDirectionX, TargetX,
				_model.LocomotionSettings.ANIMATION_DAMP_TIME * Time.deltaTime);
			_model.LocomotionRuntimeData.strafeDirectionZ = Mathf.Round(_model.LocomotionRuntimeData.strafeDirectionZ * 1000f) / 1000f;
			_model.LocomotionRuntimeData.strafeDirectionX = Mathf.Round(_model.LocomotionRuntimeData.strafeDirectionX * 1000f) / 1000f;
		}
	}
}