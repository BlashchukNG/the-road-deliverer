using UnityEngine;

namespace Logic.Characters.Player.Locomotions.States
{
	public sealed class Calculator
	{
		private readonly PlayerBehavior _model;

		public Calculator(PlayerBehavior model)
		{
			_model = model;
		}

		public void CalculateRotationalAdditives(float delta, bool leansActivated, bool headLookActivated, bool bodyLookActivated)
		{
			if (headLookActivated || leansActivated || bodyLookActivated)
			{
				_model.RuntimeData.currentRotation = _model.References.Controller.transform.forward;

				_model.RuntimeData.rotationRate =
					_model.RuntimeData.currentRotation != _model.RuntimeData.previousRotation
						? Vector3.SignedAngle(_model.RuntimeData.currentRotation, _model.RuntimeData.previousRotation, Vector3.up) / delta * -1f
						: 0f;
			}

			_model.RuntimeData.initialLeanValue = leansActivated ? _model.RuntimeData.rotationRate : 0f;

			var leanSmoothness = 5;
			var maxLeanRotationRate = 275.0f;

			var referenceValue = _model.RuntimeData.speed2D / _model.Characteristics.SprintSpeed;
			_model.RuntimeData.leanValue = CalculateSmoothedValue(
				_model.RuntimeData.leanValue,
				_model.RuntimeData.initialLeanValue,
				maxLeanRotationRate,
				leanSmoothness,
				_model.Settings.leanCurve,
				referenceValue,
				true
			);

			var headTurnSmoothness = 5f;

			if (headLookActivated && _model.RuntimeData.isTurningInPlace)
			{
				_model.RuntimeData.initialTurnValue = _model.Settings.cameraRotationOffset;
				_model.RuntimeData.headLookX = Mathf.Lerp(_model.RuntimeData.headLookX,
					_model.RuntimeData.initialTurnValue / 200, 5f * Time.deltaTime);
			}
			else
			{
				_model.RuntimeData.initialTurnValue = headLookActivated ? _model.RuntimeData.rotationRate : 0f;
				_model.RuntimeData.headLookX = CalculateSmoothedValue(
					_model.RuntimeData.headLookX,
					_model.RuntimeData.initialTurnValue,
					maxLeanRotationRate,
					headTurnSmoothness,
					_model.Settings.headLookXCurve,
					_model.RuntimeData.headLookX,
					false
				);
			}

			var bodyTurnSmoothness = 5f;

			_model.RuntimeData.initialTurnValue = bodyLookActivated ? _model.RuntimeData.rotationRate : 0f;

			_model.RuntimeData.bodyLookX = CalculateSmoothedValue(
				_model.RuntimeData.bodyLookX,
				_model.RuntimeData.initialTurnValue,
				maxLeanRotationRate,
				bodyTurnSmoothness,
				_model.Settings.bodyLookXCurve,
				_model.RuntimeData.bodyLookX,
				false
			);

			var cameraTilt = _model.References.Camera.GetCameraTiltX();
			cameraTilt = (cameraTilt > 180f ? cameraTilt - 360f : cameraTilt) / -180;
			cameraTilt = Mathf.Clamp(cameraTilt, -0.1f, 1.0f);
			_model.RuntimeData.headLookY = cameraTilt;
			_model.RuntimeData.bodyLookY = cameraTilt;

			_model.RuntimeData.previousRotation = _model.RuntimeData.currentRotation;
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
			if (!_model.RuntimeData.isGrounded)
				_model.RuntimeData.targetMaxSpeed = _model.RuntimeData.currentMaxSpeed;
			else if (_model.RuntimeData.isCrouching)
				_model.RuntimeData.targetMaxSpeed = _model.Characteristics.WalkSpeed;
			else if (_model.RuntimeData.isSprinting)
				_model.RuntimeData.targetMaxSpeed = _model.Characteristics.SprintSpeed;
			else if (_model.RuntimeData.isWalking)
				_model.RuntimeData.targetMaxSpeed = _model.Characteristics.WalkSpeed;
			else
				_model.RuntimeData.targetMaxSpeed = _model.Characteristics.RunSpeed;

			_model.RuntimeData.currentMaxSpeed =
				Mathf.Lerp(_model.RuntimeData.currentMaxSpeed, _model.RuntimeData.targetMaxSpeed,
					_model.Settings.ANIMATION_DAMP_TIME * Time.deltaTime);

			_model.RuntimeData.targetVelocity.x = _model.RuntimeData.moveDirection.x * _model.RuntimeData.currentMaxSpeed;
			_model.RuntimeData.targetVelocity.z = _model.RuntimeData.moveDirection.z * _model.RuntimeData.currentMaxSpeed;

			_model.RuntimeData.velocity.z =
				Mathf.Lerp(_model.RuntimeData.velocity.z, _model.RuntimeData.targetVelocity.z,
					_model.Settings.speedChangeDamping * Time.deltaTime);

			_model.RuntimeData.velocity.x =
				Mathf.Lerp(_model.RuntimeData.velocity.x, _model.RuntimeData.targetVelocity.x,
					_model.Settings.speedChangeDamping * Time.deltaTime);

			_model.RuntimeData.speed2D = new Vector3(_model.RuntimeData.velocity.x, 0f, _model.RuntimeData.velocity.z).magnitude;
			_model.RuntimeData.speed2D = Mathf.Round(_model.RuntimeData.speed2D * 1000f) / 1000f;

			var playerForwardVector = _model.References.Controller.transform.forward;

			_model.RuntimeData.newDirectionDifferenceAngle =
				playerForwardVector != _model.RuntimeData.moveDirection
					? Vector3.SignedAngle(playerForwardVector, _model.RuntimeData.moveDirection, Vector3.up)
					: 0f;

			CalculateGait();
		}
		
		private void CalculateGait()
		{
			var runThreshold = (_model.Characteristics.WalkSpeed + _model.Characteristics.RunSpeed) / 2;
			var sprintThreshold = (_model.Characteristics.RunSpeed + _model.Characteristics.SprintSpeed) / 2;

			if (_model.RuntimeData.speed2D < 0.01)
				_model.RuntimeData.currentGait = GaitState.Idle;
			else if (_model.RuntimeData.speed2D < runThreshold)
				_model.RuntimeData.currentGait = GaitState.Walk;
			else if (_model.RuntimeData.speed2D < sprintThreshold)
				_model.RuntimeData.currentGait = GaitState.Run;
			else
				_model.RuntimeData.currentGait = GaitState.Sprint;
		}
		
		public void CalculateFaceMoveDirection(float delta)
		{
			var characterForward = new Vector3(_model.References.Controller.transform.forward.x, 0f, 
				_model.References.Controller.transform.forward.z).normalized;
			var characterRight = new Vector3(_model.References.Controller.transform.right.x, 0f,
				_model.References.Controller.transform.right.z).normalized;
			var directionForward = new Vector3(_model.RuntimeData.moveDirection.x, 0f, _model.RuntimeData.moveDirection.z).normalized;

			_model.RuntimeData.cameraForward = _model.References.Camera.GetCameraForwardZeroedYNormalised();
			var strafingTargetRotation = Quaternion.LookRotation(_model.RuntimeData.cameraForward);

			_model.RuntimeData.strafeAngle = characterForward != directionForward ? Vector3.SignedAngle(characterForward, directionForward, Vector3.up) : 0f;

			_model.RuntimeData.isTurningInPlace = false;

			if (_model.RuntimeData.isStrafing)
			{
				if (_model.RuntimeData.moveDirection.magnitude > 0.01)
				{
					if (_model.RuntimeData.cameraForward != Vector3.zero)
					{
						_model.Settings.shuffleDirectionZ = Vector3.Dot(characterForward, directionForward);
						_model.Settings.shuffleDirectionX = Vector3.Dot(characterRight, directionForward);

						UpdateStrafeDirection(
							Vector3.Dot(characterForward, directionForward),
							Vector3.Dot(characterRight, directionForward)
						);
						_model.Settings.cameraRotationOffset = Mathf.Lerp(_model.Settings.cameraRotationOffset, 0f,
							_model.Settings.rotationSmoothing * delta);

						var targetValue = _model.RuntimeData.strafeAngle > _model.Settings.forwardStrafeMinThreshold 
						                  && _model.RuntimeData.strafeAngle < _model.Settings.forwardStrafeMaxThreshold ? 1f : 0f;

						if (Mathf.Abs(_model.Settings.forwardStrafe - targetValue) <= 0.001f)
							_model.Settings.forwardStrafe = targetValue;
						else
						{
							float t = Mathf.Clamp01(_model.Settings.STRAFE_DIRECTION_DAMP_TIME * delta);
							_model.Settings.forwardStrafe = Mathf.SmoothStep(_model.Settings.forwardStrafe, targetValue, t);
						}
					}

					_model.References.Controller.transform.rotation = Quaternion.Slerp(_model.References.Controller.transform.rotation, 
						strafingTargetRotation, _model.Settings.rotationSmoothing * delta);
				}
				else
				{
					UpdateStrafeDirection(1f, 0f);

					var t = 20 * delta;
					var newOffset = 0f;

					if (characterForward != _model.RuntimeData.cameraForward) newOffset = Vector3.SignedAngle(characterForward, 
						_model.RuntimeData.cameraForward, Vector3.up);

					_model.Settings.cameraRotationOffset = Mathf.Lerp(_model.Settings.cameraRotationOffset, newOffset, t);

					if (Mathf.Abs(_model.Settings.cameraRotationOffset) > 10)
					{
						_model.RuntimeData.isTurningInPlace = true;
					}
				}
			}
			else
			{
				UpdateStrafeDirection(1f, 0f);
				_model.Settings.cameraRotationOffset = Mathf.Lerp(_model.Settings.cameraRotationOffset, 0f, 
					_model.Settings.rotationSmoothing * delta);

				_model.Settings.shuffleDirectionZ = 1;
				_model.Settings.shuffleDirectionX = 0;

				var faceDirection = new Vector3(_model.RuntimeData.velocity.x, 0f, _model.RuntimeData.velocity.z);

				if (faceDirection == Vector3.zero)
					return;

				_model.References.Controller.transform.rotation = Quaternion.Slerp(
					_model.References.Controller.transform.rotation,
					Quaternion.LookRotation(faceDirection),
					_model.Settings.rotationSmoothing * Time.deltaTime
				);
			}
		}
		
		private void UpdateStrafeDirection(float TargetZ, float TargetX)
		{
			_model.RuntimeData.strafeDirectionZ = Mathf.Lerp(_model.RuntimeData.strafeDirectionZ, TargetZ,
				_model.Settings.ANIMATION_DAMP_TIME * Time.deltaTime);
			_model.RuntimeData.strafeDirectionX = Mathf.Lerp(_model.RuntimeData.strafeDirectionX, TargetX,
				_model.Settings.ANIMATION_DAMP_TIME * Time.deltaTime);
			_model.RuntimeData.strafeDirectionZ = Mathf.Round(_model.RuntimeData.strafeDirectionZ * 1000f) / 1000f;
			_model.RuntimeData.strafeDirectionX = Mathf.Round(_model.RuntimeData.strafeDirectionX * 1000f) / 1000f;
		}
	}
}