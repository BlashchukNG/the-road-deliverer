using UnityEngine;

namespace Logic.Characters.Player.Locomotions.States
{
	public sealed class Checker
	{
		private readonly PlayerBehavior _model;

		public Checker(PlayerBehavior model)
		{
			_model = model;
		}

		public void GroundCheck()
		{
			Vector3 spherePosition = new Vector3(
				_model.References.Controller.transform.position.x,
				_model.References.Controller.transform.position.y - _model.Settings.groundedOffset,
				_model.References.Controller.transform.position.z
			);
			
			_model.RuntimeData.isGrounded = 
				Physics.CheckSphere(spherePosition, _model.References.Controller.radius,
					_model.Settings.groundLayerMask, QueryTriggerInteraction.Ignore);

			if (_model.RuntimeData.isGrounded) 
				GroundInclineCheck();
		}
		
		private void GroundInclineCheck()
		{
			float rayDistance = Mathf.Infinity;
			_model.References.RearRayPos.rotation = Quaternion.Euler(_model.References.Controller.transform.rotation.x, 0, 0);
			_model.References.FrontRayPos.rotation = Quaternion.Euler(_model.References.Controller.transform.rotation.x, 0, 0);

			Physics.Raycast(_model.References.RearRayPos.position,
				_model.References.RearRayPos.TransformDirection(-Vector3.up), 
				out RaycastHit rearHit, rayDistance, 
				_model.Settings.groundLayerMask);
			
			Physics.Raycast(_model.References.FrontRayPos.position,
				_model.References.FrontRayPos.TransformDirection(-Vector3.up),
				out RaycastHit frontHit,
				rayDistance,
				_model.Settings.groundLayerMask
			);

			Vector3 hitDifference = frontHit.point - rearHit.point;
			float xPlaneLength = new Vector2(hitDifference.x, hitDifference.z).magnitude;

			_model.RuntimeData.inclineAngle = Mathf.Lerp(_model.RuntimeData.inclineAngle, 
				Mathf.Atan2(hitDifference.y, xPlaneLength) * Mathf.Rad2Deg, 20f * Time.deltaTime);
		}
		
		public void CheckEnableTurns()
		{
			_model.RuntimeData.headLookDelay = VariableOverrideDelayTimer(_model.RuntimeData.headLookDelay);
			_model.Settings.enableHeadTurn = _model.RuntimeData.headLookDelay == 0.0f && !_model.RuntimeData.isStarting;
			_model.RuntimeData.bodyLookDelay = VariableOverrideDelayTimer(_model.RuntimeData.bodyLookDelay);
			_model.Settings.enableBodyTurn = _model.RuntimeData.bodyLookDelay == 0.0f 
			                                           && !(_model.RuntimeData.isStarting || _model.RuntimeData.isTurningInPlace);
		}

		public void CheckEnableLean()
		{
			_model.RuntimeData.leanDelay = VariableOverrideDelayTimer(_model.RuntimeData.leanDelay);
			_model.Settings.enableLean = _model.RuntimeData.leanDelay == 0.0f 
			                                       && !(_model.RuntimeData.isStarting || _model.RuntimeData.isTurningInPlace);
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
			_model.RuntimeData.locomotionStartTimer = VariableOverrideDelayTimer(_model.RuntimeData.locomotionStartTimer);

			var isStartingCheck = false;

			if (_model.RuntimeData.locomotionStartTimer <= 0.0f)
			{
				if (_model.RuntimeData.moveDirection.magnitude > 0.01 && _model.RuntimeData.speed2D < 1 && !_model.RuntimeData.isStrafing)
					isStartingCheck = true;

				if (isStartingCheck)
				{
					if (!_model.RuntimeData.isStarting)
					{
						_model.RuntimeData.locomotionStartDirection = _model.RuntimeData.newDirectionDifferenceAngle;
						_model.References.Animator.SetFloat(_model.AnimationsVariables.locomotionStartDirectionHash, 
							_model.RuntimeData.locomotionStartDirection);
					}

					float delayTime = 0.2f;
					_model.RuntimeData.leanDelay = delayTime;
					_model.RuntimeData.headLookDelay = delayTime;
					_model.RuntimeData.bodyLookDelay = delayTime;

					_model.RuntimeData.locomotionStartTimer = delayTime;
				}
			}
			else
				isStartingCheck = true;

			_model.RuntimeData.isStarting = isStartingCheck;
			_model.References.Animator.SetBool(_model.AnimationsVariables.isStartingHash, _model.RuntimeData.isStarting);
		}
		
		public void CheckIfStopped()
		{
			_model.RuntimeData.isStopped = _model.RuntimeData.moveDirection.magnitude == 0 && _model.RuntimeData.speed2D < .5;
		}
	}
}