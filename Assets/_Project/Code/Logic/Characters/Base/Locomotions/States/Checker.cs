using UnityEngine;

namespace Logic.Characters.Base.Locomotions.States
{
	public sealed class Checker
	{
		private readonly CharacterModel _model;

		public Checker(CharacterModel model)
		{
			_model = model;
		}

		public void GroundCheck()
		{
			Vector3 spherePosition = new Vector3(
				_model.LocomotionReferences.Controller.transform.position.x,
				_model.LocomotionReferences.Controller.transform.position.y - _model.LocomotionSettings.groundedOffset,
				_model.LocomotionReferences.Controller.transform.position.z
			);
			
			_model.LocomotionRuntimeData.isGrounded = 
				Physics.CheckSphere(spherePosition, _model.LocomotionReferences.Controller.radius,
					_model.LocomotionSettings.groundLayerMask, QueryTriggerInteraction.Ignore);

			if (_model.LocomotionRuntimeData.isGrounded) 
				GroundInclineCheck();
		}
		
		private void GroundInclineCheck()
		{
			float rayDistance = Mathf.Infinity;
			_model.LocomotionReferences.RearRayPos.rotation = Quaternion.Euler(_model.LocomotionReferences.Controller.transform.rotation.x, 0, 0);
			_model.LocomotionReferences.FrontRayPos.rotation = Quaternion.Euler(_model.LocomotionReferences.Controller.transform.rotation.x, 0, 0);

			Physics.Raycast(_model.LocomotionReferences.RearRayPos.position,
				_model.LocomotionReferences.RearRayPos.TransformDirection(-Vector3.up), 
				out RaycastHit rearHit, rayDistance, 
				_model.LocomotionSettings.groundLayerMask);
			
			Physics.Raycast(_model.LocomotionReferences.FrontRayPos.position,
				_model.LocomotionReferences.FrontRayPos.TransformDirection(-Vector3.up),
				out RaycastHit frontHit,
				rayDistance,
				_model.LocomotionSettings.groundLayerMask
			);

			Vector3 hitDifference = frontHit.point - rearHit.point;
			float xPlaneLength = new Vector2(hitDifference.x, hitDifference.z).magnitude;

			_model.LocomotionRuntimeData.inclineAngle = Mathf.Lerp(_model.LocomotionRuntimeData.inclineAngle, 
				Mathf.Atan2(hitDifference.y, xPlaneLength) * Mathf.Rad2Deg, 20f * Time.deltaTime);
		}
		
		public void CheckEnableTurns()
		{
			_model.LocomotionRuntimeData.headLookDelay = VariableOverrideDelayTimer(_model.LocomotionRuntimeData.headLookDelay);
			_model.LocomotionSettings.enableHeadTurn = _model.LocomotionRuntimeData.headLookDelay == 0.0f && !_model.LocomotionRuntimeData.isStarting;
			_model.LocomotionRuntimeData.bodyLookDelay = VariableOverrideDelayTimer(_model.LocomotionRuntimeData.bodyLookDelay);
			_model.LocomotionSettings.enableBodyTurn = _model.LocomotionRuntimeData.bodyLookDelay == 0.0f 
			                                           && !(_model.LocomotionRuntimeData.isStarting || _model.LocomotionRuntimeData.isTurningInPlace);
		}

		public void CheckEnableLean()
		{
			_model.LocomotionRuntimeData.leanDelay = VariableOverrideDelayTimer(_model.LocomotionRuntimeData.leanDelay);
			_model.LocomotionSettings.enableLean = _model.LocomotionRuntimeData.leanDelay == 0.0f 
			                                       && !(_model.LocomotionRuntimeData.isStarting || _model.LocomotionRuntimeData.isTurningInPlace);
		}
		
		private float VariableOverrideDelayTimer(float timeVariable)
		{
			if (timeVariable > 0.0f)
			{
				timeVariable -= Time.deltaTime;
				timeVariable = Mathf.Clamp(timeVariable, 0.0f, 1.0f);
			}
			else
				timeVariable = 0.0f;

			return timeVariable;
		}
		
		public void CheckIfStarting()
		{
			_model.LocomotionRuntimeData.locomotionStartTimer = VariableOverrideDelayTimer(_model.LocomotionRuntimeData.locomotionStartTimer);

			var isStartingCheck = false;

			if (_model.LocomotionRuntimeData.locomotionStartTimer <= 0.0f)
			{
				if (_model.LocomotionRuntimeData.moveDirection.magnitude > 0.01 && _model.LocomotionRuntimeData.speed2D < 1 && !_model.LocomotionRuntimeData.isStrafing)
					isStartingCheck = true;

				if (isStartingCheck)
				{
					if (!_model.LocomotionRuntimeData.isStarting)
					{
						_model.LocomotionRuntimeData.locomotionStartDirection = _model.LocomotionRuntimeData.newDirectionDifferenceAngle;
						_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.locomotionStartDirectionHash, 
							_model.LocomotionRuntimeData.locomotionStartDirection);
					}

					float delayTime = 0.2f;
					_model.LocomotionRuntimeData.leanDelay = delayTime;
					_model.LocomotionRuntimeData.headLookDelay = delayTime;
					_model.LocomotionRuntimeData.bodyLookDelay = delayTime;

					_model.LocomotionRuntimeData.locomotionStartTimer = delayTime;
				}
			}
			else
				isStartingCheck = true;

			_model.LocomotionRuntimeData.isStarting = isStartingCheck;
			_model.LocomotionReferences.Animator.SetBool(_model.LocomotionAnimationsVariables.isStartingHash, _model.LocomotionRuntimeData.isStarting);
		}
		
		public void CheckIfStopped()
		{
			_model.LocomotionRuntimeData.isStopped = _model.LocomotionRuntimeData.moveDirection.magnitude == 0 && _model.LocomotionRuntimeData.speed2D < .5;
		}
	}
}