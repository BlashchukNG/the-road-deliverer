using Infrastructure.Roots.AppRoot.Services.UserUnput;
using Logic.Characters.Player;
using Logic.UserCamera;
using UnityEngine;
using AnimationState = Logic.Characters.Base.Locomotions.AnimationState;

namespace Logic.Characters.Base.Locomotions
{
	public sealed class Locomotion
	{
		#region References

		private readonly PlayerView _view;
		private readonly IUserInputService _input;
		private readonly CameraController _camera;
		private readonly CharacterController _controller;
		private readonly Animator _animator;

		#endregion

		#region Animation Variable Hashes

		private readonly int _movementInputTappedHash = Animator.StringToHash("MovementInputTapped");
		private readonly int _movementInputPressedHash = Animator.StringToHash("MovementInputPressed");
		private readonly int _movementInputHeldHash = Animator.StringToHash("MovementInputHeld");
		private readonly int _shuffleDirectionXHash = Animator.StringToHash("ShuffleDirectionX");
		private readonly int _shuffleDirectionZHash = Animator.StringToHash("ShuffleDirectionZ");

		private readonly int _moveSpeedHash = Animator.StringToHash("MoveSpeed");
		private readonly int _currentGaitHash = Animator.StringToHash("CurrentGait");

		private readonly int _isJumpingAnimHash = Animator.StringToHash("IsJumping");
		private readonly int _fallingDurationHash = Animator.StringToHash("FallingDuration");

		private readonly int _inclineAngleHash = Animator.StringToHash("InclineAngle");

		private readonly int _strafeDirectionXHash = Animator.StringToHash("StrafeDirectionX");
		private readonly int _strafeDirectionZHash = Animator.StringToHash("StrafeDirectionZ");

		private readonly int _forwardStrafeHash = Animator.StringToHash("ForwardStrafe");
		private readonly int _cameraRotationOffsetHash = Animator.StringToHash("CameraRotationOffset");
		private readonly int _isStrafingHash = Animator.StringToHash("IsStrafing");
		private readonly int _isTurningInPlaceHash = Animator.StringToHash("IsTurningInPlace");

		private readonly int _isCrouchingHash = Animator.StringToHash("IsCrouching");

		private readonly int _isWalkingHash = Animator.StringToHash("IsWalking");
		private readonly int _isStoppedHash = Animator.StringToHash("IsStopped");
		private readonly int _isStartingHash = Animator.StringToHash("IsStarting");

		private readonly int _isGroundedHash = Animator.StringToHash("IsGrounded");

		private readonly int _leanValueHash = Animator.StringToHash("LeanValue");
		private readonly int _headLookXHash = Animator.StringToHash("HeadLookX");
		private readonly int _headLookYHash = Animator.StringToHash("HeadLookY");

		private readonly int _bodyLookXHash = Animator.StringToHash("BodyLookX");
		private readonly int _bodyLookYHash = Animator.StringToHash("BodyLookY");

		private readonly int _locomotionStartDirectionHash = Animator.StringToHash("LocomotionStartDirection");

		#endregion

		#region Locomotion Settings

		private float _walkSpeed = 1.4f;
		private float _runSpeed = 2.5f;
		private float _sprintSpeed = 6f;
		private float _speedChangeDamping = 10f;
		private float _rotationSmoothing = 10f;
		private float _cameraRotationOffset;
		private float _buttonHoldThreshold = 0.15f;
		private float _shuffleDirectionX;
		private float _shuffleDirectionZ;
		private bool _alwaysStrafe = true;

		#endregion

		#region Capsule Settings

		private float _capsuleStandingHeight = 1.8f;
		private float _capsuleStandingCentre = 0.93f;
		private float _capsuleCrouchingHeight = 1.2f;
		private float _capsuleCrouchingCentre = 0.6f;

		#endregion

		#region Strafing

		private float _forwardStrafeMinThreshold = -55.0f;
		private float _forwardStrafeMaxThreshold = 125.0f;
		private float _forwardStrafe = 1f;

		#endregion

		#region Grounded Settings

		private Transform _rearRayPos;
		private Transform _frontRayPos;
		private LayerMask _groundLayerMask = LayerMask.GetMask("ground");
		private float _inclineAngle;
		private float _groundedOffset = -0.14f;

		#endregion

		#region In-Air Settings

		private float _jumpForce = 10f;
		private float _gravityMultiplier = 2f;
		private float _fallingDuration;

		#endregion

		#region Head Look Settings

		private bool _enableHeadTurn = true;
		private float _headLookDelay;
		private float _headLookX;
		private float _headLookY;
		private AnimationCurve _headLookXCurve;

		#endregion

		#region Body Look Settings

		private bool _enableBodyTurn = true;
		private float _bodyLookDelay;
		private float _bodyLookX;
		private float _bodyLookY;
		private AnimationCurve _bodyLookXCurve;

		#endregion

		#region Lean Settings

		private bool _enableLean = true;
		private float _leanDelay;
		private float _leanValue;
		private AnimationCurve _leanCurve;
		private float _leansHeadLooksDelay;
		private bool _animationClipEnd;

		#endregion

		#region Runtime Properties

		private AnimationState _currentState = AnimationState.Base;
		private GaitState _currentGait;
		private Vector3 _currentRotation;
		private Vector3 _moveDirection;
		private Vector3 _previousRotation;
		private Vector3 _velocity;
		private float _currentMaxSpeed;
		private float _locomotionStartDirection;
		private float _locomotionStartTimer;
		private float _lookingAngle;
		private float _newDirectionDifferenceAngle;
		private float _speed2D;
		private float _strafeAngle;
		private float _strafeDirectionX;
		private float _strafeDirectionZ;
		private bool _cannotStandUp;
		private bool _crouchKeyPressed;
		private bool _isCrouching;
		private bool _isGrounded = true;
		private bool _isSliding;
		private bool _isSprinting;
		private bool _isStarting;
		private bool _isStopped = true;
		private bool _isStrafing;
		private bool _isTurningInPlace;
		private bool _isWalking;
		private bool _movementInputHeld;
		private bool _movementInputPressed;
		private bool _movementInputTapped;

		#endregion

		#region Base State Variables

		private const float _ANIMATION_DAMP_TIME = 5f;
		private const float _STRAFE_DIRECTION_DAMP_TIME = 20f;
		private float _targetMaxSpeed;
		private float _fallStartTime;
		private float _rotationRate;
		private float _initialLeanValue;
		private float _initialTurnValue;
		private Vector3 _cameraForward;
		private Vector3 _targetVelocity;

		#endregion

		public Locomotion(PlayerView view, IUserInputService input, CameraController camera)
		{
			_view = view;
			_input = input;
			_camera = camera;
			_controller = view.CharacterController;
			_animator = view.Animator;

			_frontRayPos = view.frontRayPos;
			_rearRayPos = view.rearRayPos;

			_headLookXCurve = view.headLookXCurve;
			_bodyLookXCurve = view.bodyLookXCurve;
			_leanCurve = view.leanCurve;

			_isStrafing = _alwaysStrafe;

			_input.onWalkToggled += ToggleWalk;
			_input.onSprintActivated += ActivateSprint;
			_input.onSprintDeactivated += DeactivateSprint;
			_input.onCrouchActivated += ActivateCrouch;
			_input.onCrouchDeactivated += DeactivateCrouch;

			SwitchState(AnimationState.Locomotion);
		}

		public void Tick(float delta)
		{
			switch (_currentState)
			{
				case AnimationState.Locomotion:
					UpdateLocomotionState(delta);
					break;
				case AnimationState.Jump:
					UpdateJumpState(delta);
					break;
				case AnimationState.Fall:
					UpdateFallState(delta);
					break;
				case AnimationState.Crouch:
					UpdateCrouchState(delta);
					break;
			}
		}

		private void UpdateAnimatorController()
		{
			_animator.SetFloat(_leanValueHash, _leanValue);
			_animator.SetFloat(_headLookXHash, _headLookX);
			_animator.SetFloat(_headLookYHash, _headLookY);
			_animator.SetFloat(_bodyLookXHash, _bodyLookX);
			_animator.SetFloat(_bodyLookYHash, _bodyLookY);

			_animator.SetFloat(_isStrafingHash, _isStrafing ? 1.0f : 0.0f);

			_animator.SetFloat(_inclineAngleHash, _inclineAngle);

			_animator.SetFloat(_moveSpeedHash, _speed2D);
			_animator.SetInteger(_currentGaitHash, (int)_currentGait);

			_animator.SetFloat(_strafeDirectionXHash, _strafeDirectionX);
			_animator.SetFloat(_strafeDirectionZHash, _strafeDirectionZ);
			_animator.SetFloat(_forwardStrafeHash, _forwardStrafe);
			_animator.SetFloat(_cameraRotationOffsetHash, _cameraRotationOffset);

			_animator.SetBool(_movementInputHeldHash, _movementInputHeld);
			_animator.SetBool(_movementInputPressedHash, _movementInputPressed);
			_animator.SetBool(_movementInputTappedHash, _movementInputTapped);
			_animator.SetFloat(_shuffleDirectionXHash, _shuffleDirectionX);
			_animator.SetFloat(_shuffleDirectionZHash, _shuffleDirectionZ);

			_animator.SetBool(_isTurningInPlaceHash, _isTurningInPlace);
			_animator.SetBool(_isCrouchingHash, _isCrouching);

			_animator.SetFloat(_fallingDurationHash, _fallingDuration);
			_animator.SetBool(_isGroundedHash, _isGrounded);

			_animator.SetBool(_isWalkingHash, _isWalking);
			_animator.SetBool(_isStoppedHash, _isStopped);

			_animator.SetFloat(_locomotionStartDirectionHash, _locomotionStartDirection);
		}

		#region State Change

		private void SwitchState(AnimationState newState)
		{
			ExitCurrentState();
			EnterState(newState);
		}

		private void EnterState(AnimationState stateToEnter)
		{
			_currentState = stateToEnter;
			switch (_currentState)
			{
				case AnimationState.Base:
					EnterBaseState();
					break;
				case AnimationState.Locomotion:
					EnterLocomotionState();
					break;
				case AnimationState.Jump:
					EnterJumpState();
					break;
				case AnimationState.Fall:
					EnterFallState();
					break;
				case AnimationState.Crouch:
					EnterCrouchState();
					break;
			}
		}

		private void ExitCurrentState()
		{
			switch (_currentState)
			{
				case AnimationState.Locomotion:
					ExitLocomotionState();
					break;
				case AnimationState.Jump:
					ExitJumpState();
					break;
				case AnimationState.Crouch:
					ExitCrouchState();
					break;
			}
		}

		#endregion

		#region Locomotion State

		private void EnterLocomotionState()
		{
			_input.onJumpPerformed += LocomotionToJumpState;
		}

		private void UpdateLocomotionState(float delta)
		{
			GroundedCheck();

			if (!_isGrounded) SwitchState(AnimationState.Fall);
			if (_isCrouching) SwitchState(AnimationState.Crouch);

			CheckEnableTurns();
			CheckEnableLean();
			CalculateRotationalAdditives(_enableLean, _enableHeadTurn, _enableBodyTurn);

			CalculateMoveDirection();
			CheckIfStarting();
			CheckIfStopped();
			FaceMoveDirection();
			Move(delta);
			UpdateAnimatorController();
		}

		private void Move(float delta)
		{
			_controller.Move(_velocity * delta);
		}

		private void FaceMoveDirection()
		{
			Vector3 characterForward = new Vector3(_controller.transform.forward.x, 0f, _controller.transform.forward.z).normalized;
			Vector3 characterRight = new Vector3(_controller.transform.right.x, 0f, _controller.transform.right.z).normalized;
			Vector3 directionForward = new Vector3(_moveDirection.x, 0f, _moveDirection.z).normalized;

			_cameraForward = _camera.GetCameraForwardZeroedYNormalised();
			Quaternion strafingTargetRotation = Quaternion.LookRotation(_cameraForward);

			_strafeAngle = characterForward != directionForward ? Vector3.SignedAngle(characterForward, directionForward, Vector3.up) : 0f;

			_isTurningInPlace = false;

			if (_isStrafing)
			{
				if (_moveDirection.magnitude > 0.01)
				{
					if (_cameraForward != Vector3.zero)
					{
						_shuffleDirectionZ = Vector3.Dot(characterForward, directionForward);
						_shuffleDirectionX = Vector3.Dot(characterRight, directionForward);

						UpdateStrafeDirection(
							Vector3.Dot(characterForward, directionForward),
							Vector3.Dot(characterRight, directionForward)
						);
						_cameraRotationOffset = Mathf.Lerp(_cameraRotationOffset, 0f, _rotationSmoothing * Time.deltaTime);

						float targetValue = _strafeAngle > _forwardStrafeMinThreshold && _strafeAngle < _forwardStrafeMaxThreshold ? 1f : 0f;

						if (Mathf.Abs(_forwardStrafe - targetValue) <= 0.001f)
						{
							_forwardStrafe = targetValue;
						}
						else
						{
							float t = Mathf.Clamp01(_STRAFE_DIRECTION_DAMP_TIME * Time.deltaTime);
							_forwardStrafe = Mathf.SmoothStep(_forwardStrafe, targetValue, t);
						}
					}

					_controller.transform.rotation = Quaternion.Slerp(_controller.transform.rotation, strafingTargetRotation, _rotationSmoothing * Time.deltaTime);
				}
				else
				{
					UpdateStrafeDirection(1f, 0f);

					float t = 20 * Time.deltaTime;
					float newOffset = 0f;

					if (characterForward != _cameraForward) newOffset = Vector3.SignedAngle(characterForward, _cameraForward, Vector3.up);

					_cameraRotationOffset = Mathf.Lerp(_cameraRotationOffset, newOffset, t);

					if (Mathf.Abs(_cameraRotationOffset) > 10)
					{
						_isTurningInPlace = true;
					}
				}
			}
			else
			{
				UpdateStrafeDirection(1f, 0f);
				_cameraRotationOffset = Mathf.Lerp(_cameraRotationOffset, 0f, _rotationSmoothing * Time.deltaTime);

				_shuffleDirectionZ = 1;
				_shuffleDirectionX = 0;

				Vector3 faceDirection = new Vector3(_velocity.x, 0f, _velocity.z);

				if (faceDirection == Vector3.zero)
					return;

				_controller.transform.rotation = Quaternion.Slerp(
					_controller.transform.rotation,
					Quaternion.LookRotation(faceDirection),
					_rotationSmoothing * Time.deltaTime
				);
			}
		}

		private void UpdateStrafeDirection(float TargetZ, float TargetX)
		{
			_strafeDirectionZ = Mathf.Lerp(_strafeDirectionZ, TargetZ, _ANIMATION_DAMP_TIME * Time.deltaTime);
			_strafeDirectionX = Mathf.Lerp(_strafeDirectionX, TargetX, _ANIMATION_DAMP_TIME * Time.deltaTime);
			_strafeDirectionZ = Mathf.Round(_strafeDirectionZ * 1000f) / 1000f;
			_strafeDirectionX = Mathf.Round(_strafeDirectionX * 1000f) / 1000f;
		}

		private void CheckIfStarting()
		{
			_locomotionStartTimer = VariableOverrideDelayTimer(_locomotionStartTimer);

			bool isStartingCheck = false;

			if (_locomotionStartTimer <= 0.0f)
			{
				if (_moveDirection.magnitude > 0.01 && _speed2D < 1 && !_isStrafing)
				{
					isStartingCheck = true;
				}

				if (isStartingCheck)
				{
					if (!_isStarting)
					{
						_locomotionStartDirection = _newDirectionDifferenceAngle;
						_animator.SetFloat(_locomotionStartDirectionHash, _locomotionStartDirection);
					}

					float delayTime = 0.2f;
					_leanDelay = delayTime;
					_headLookDelay = delayTime;
					_bodyLookDelay = delayTime;

					_locomotionStartTimer = delayTime;
				}
			}
			else
			{
				isStartingCheck = true;
			}

			_isStarting = isStartingCheck;
			_animator.SetBool(_isStartingHash, _isStarting);
		}

		private void CheckIfStopped()
		{
			_isStopped = _moveDirection.magnitude == 0 && _speed2D < .5;
		}

		private void GroundedCheck()
		{
			Vector3 spherePosition = new Vector3(
				_controller.transform.position.x,
				_controller.transform.position.y - _groundedOffset,
				_controller.transform.position.z
			);
			_isGrounded = Physics.CheckSphere(spherePosition, _controller.radius, _groundLayerMask, QueryTriggerInteraction.Ignore);

			if (_isGrounded)
			{
				GroundInclineCheck();
			}
		}

		private void GroundInclineCheck()
		{
			float rayDistance = Mathf.Infinity;
			_rearRayPos.rotation = Quaternion.Euler(_controller.transform.rotation.x, 0, 0);
			_frontRayPos.rotation = Quaternion.Euler(_controller.transform.rotation.x, 0, 0);

			Physics.Raycast(_rearRayPos.position, _rearRayPos.TransformDirection(-Vector3.up), out RaycastHit rearHit, rayDistance, _groundLayerMask);
			Physics.Raycast(
				_frontRayPos.position,
				_frontRayPos.TransformDirection(-Vector3.up),
				out RaycastHit frontHit,
				rayDistance,
				_groundLayerMask
			);

			Vector3 hitDifference = frontHit.point - rearHit.point;
			float xPlaneLength = new Vector2(hitDifference.x, hitDifference.z).magnitude;

			_inclineAngle = Mathf.Lerp(_inclineAngle, Mathf.Atan2(hitDifference.y, xPlaneLength) * Mathf.Rad2Deg, 20f * Time.deltaTime);
		}

		private void ExitLocomotionState()
		{
			_input.onJumpPerformed -= LocomotionToJumpState;
		}

		private void LocomotionToJumpState()
		{
			SwitchState(AnimationState.Jump);
		}

		#endregion

		#region Movement

		private void CalculateMoveDirection()
		{
			CalculateInput();

			if (!_isGrounded)
				_targetMaxSpeed = _currentMaxSpeed;
			else if (_isCrouching)
				_targetMaxSpeed = _walkSpeed;
			else if (_isSprinting)
				_targetMaxSpeed = _sprintSpeed;
			else if (_isWalking)
				_targetMaxSpeed = _walkSpeed;
			else
				_targetMaxSpeed = _runSpeed;

			_currentMaxSpeed = Mathf.Lerp(_currentMaxSpeed, _targetMaxSpeed, _ANIMATION_DAMP_TIME * Time.deltaTime);

			_targetVelocity.x = _moveDirection.x * _currentMaxSpeed;
			_targetVelocity.z = _moveDirection.z * _currentMaxSpeed;

			_velocity.z = Mathf.Lerp(_velocity.z, _targetVelocity.z, _speedChangeDamping * Time.deltaTime);
			_velocity.x = Mathf.Lerp(_velocity.x, _targetVelocity.x, _speedChangeDamping * Time.deltaTime);

			_speed2D = new Vector3(_velocity.x, 0f, _velocity.z).magnitude;
			_speed2D = Mathf.Round(_speed2D * 1000f) / 1000f;

			Vector3 playerForwardVector = _controller.transform.forward;

			_newDirectionDifferenceAngle = playerForwardVector != _moveDirection
				                               ? Vector3.SignedAngle(playerForwardVector, _moveDirection, Vector3.up)
				                               : 0f;

			CalculateGait();
		}

		private void ApplyGravity()
		{
			if (_velocity.y > Physics.gravity.y)
			{
				_velocity.y += Physics.gravity.y * _gravityMultiplier * Time.deltaTime;
			}
		}

		private void CalculateGait()
		{
			float runThreshold = (_walkSpeed + _runSpeed) / 2;
			float sprintThreshold = (_runSpeed + _sprintSpeed) / 2;

			if (_speed2D < 0.01)
				_currentGait = GaitState.Idle;
			else if (_speed2D < runThreshold)
				_currentGait = GaitState.Walk;
			else if (_speed2D < sprintThreshold)
				_currentGait = GaitState.Run;
			else
				_currentGait = GaitState.Sprint;
		}

		#endregion

		#region Checks

		private void CheckEnableTurns()
		{
			_headLookDelay = VariableOverrideDelayTimer(_headLookDelay);
			_enableHeadTurn = _headLookDelay == 0.0f && !_isStarting;
			_bodyLookDelay = VariableOverrideDelayTimer(_bodyLookDelay);
			_enableBodyTurn = _bodyLookDelay == 0.0f && !(_isStarting || _isTurningInPlace);
		}

		private void CheckEnableLean()
		{
			_leanDelay = VariableOverrideDelayTimer(_leanDelay);
			_enableLean = _leanDelay == 0.0f && !(_isStarting || _isTurningInPlace);
		}

		#endregion

		#region Lean and Offsets

		private void CalculateRotationalAdditives(bool leansActivated, bool headLookActivated, bool bodyLookActivated)
		{
			if (headLookActivated || leansActivated || bodyLookActivated)
			{
				_currentRotation = _controller.transform.forward;

				_rotationRate = _currentRotation != _previousRotation
					                ? Vector3.SignedAngle(_currentRotation, _previousRotation, Vector3.up) / Time.deltaTime * -1f
					                : 0f;
			}

			_initialLeanValue = leansActivated ? _rotationRate : 0f;

			float leanSmoothness = 5;
			float maxLeanRotationRate = 275.0f;

			float referenceValue = _speed2D / _sprintSpeed;
			_leanValue = CalculateSmoothedValue(
				_leanValue,
				_initialLeanValue,
				maxLeanRotationRate,
				leanSmoothness,
				_leanCurve,
				referenceValue,
				true
			);

			float headTurnSmoothness = 5f;

			if (headLookActivated && _isTurningInPlace)
			{
				_initialTurnValue = _cameraRotationOffset;
				_headLookX = Mathf.Lerp(_headLookX, _initialTurnValue / 200, 5f * Time.deltaTime);
			}
			else
			{
				_initialTurnValue = headLookActivated ? _rotationRate : 0f;
				_headLookX = CalculateSmoothedValue(
					_headLookX,
					_initialTurnValue,
					maxLeanRotationRate,
					headTurnSmoothness,
					_headLookXCurve,
					_headLookX,
					false
				);
			}

			float bodyTurnSmoothness = 5f;

			_initialTurnValue = bodyLookActivated ? _rotationRate : 0f;

			_bodyLookX = CalculateSmoothedValue(
				_bodyLookX,
				_initialTurnValue,
				maxLeanRotationRate,
				bodyTurnSmoothness,
				_bodyLookXCurve,
				_bodyLookX,
				false
			);

			float cameraTilt = _camera.GetCameraTiltX();
			cameraTilt = (cameraTilt > 180f ? cameraTilt - 360f : cameraTilt) / -180;
			cameraTilt = Mathf.Clamp(cameraTilt, -0.1f, 1.0f);
			_headLookY = cameraTilt;
			_bodyLookY = cameraTilt;

			_previousRotation = _currentRotation;
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
			float changeVariable = newValue / maxRateChange;

			changeVariable = Mathf.Clamp(changeVariable, -1.0f, 1.0f);

			if (isMultiplier)
			{
				float multiplier = referenceCurve.Evaluate(referenceValue);
				changeVariable *= multiplier;
			}
			else
			{
				changeVariable = referenceCurve.Evaluate(changeVariable);
			}

			if (!changeVariable.Equals(mainVariable))
			{
				changeVariable = Mathf.Lerp(mainVariable, changeVariable, smoothness * Time.deltaTime);
			}

			return changeVariable;
		}

		private float VariableOverrideDelayTimer(float timeVariable)
		{
			if (timeVariable > 0.0f)
			{
				timeVariable -= Time.deltaTime;
				timeVariable = Mathf.Clamp(timeVariable, 0.0f, 1.0f);
			}
			else
			{
				timeVariable = 0.0f;
			}

			return timeVariable;
		}

		#endregion

		#region Walk

		private void ToggleWalk()
		{
			EnableWalk(!_isWalking);
		}

		private void EnableWalk(bool enable)
		{
			_isWalking = enable && _isGrounded && !_isSprinting;
		}

		#endregion

		#region Sprint

		private void ActivateSprint()
		{
			if (!_isCrouching)
			{
				EnableWalk(false);
				_isSprinting = true;
				_isStrafing = false;
			}
		}

		private void DeactivateSprint()
		{
			_isSprinting = false;

			if (_alwaysStrafe)
			{
				_isStrafing = true;
			}
		}

		#endregion

		#region Crouch

		private void ActivateCrouch()
		{
			_crouchKeyPressed = true;

			if (_isGrounded)
			{
				CapsuleCrouchingSize(true);
				DeactivateSprint();
				_isCrouching = true;
			}
		}

		private void DeactivateCrouch()
		{
			_crouchKeyPressed = false;

			if (!_cannotStandUp && !_isSliding)
			{
				CapsuleCrouchingSize(false);
				_isCrouching = false;
			}
		}

		public void ActivateSliding()
		{
			_isSliding = true;
		}

		public void DeactivateSliding()
		{
			_isSliding = false;
		}

		private void CapsuleCrouchingSize(bool crouching)
		{
			if (crouching)
			{
				_controller.center = new Vector3(0f, _capsuleCrouchingCentre, 0f);
				_controller.height = _capsuleCrouchingHeight;
			}
			else
			{
				_controller.center = new Vector3(0f, _capsuleStandingCentre, 0f);
				_controller.height = _capsuleStandingHeight;
			}
		}

		#endregion

		#region Jump State

		private void EnterJumpState()
		{
			_animator.SetBool(_isJumpingAnimHash, true);

			_isSliding = false;

			_velocity = new Vector3(_velocity.x, _jumpForce, _velocity.z);
		}

		private void UpdateJumpState(float delta)
		{
			ApplyGravity();

			if (_velocity.y <= 0f)
			{
				_animator.SetBool(_isJumpingAnimHash, false);
				SwitchState(AnimationState.Fall);
			}

			GroundedCheck();

			CalculateRotationalAdditives(false, _enableHeadTurn, _enableBodyTurn);
			CalculateMoveDirection();
			FaceMoveDirection();
			Move(delta);
			UpdateAnimatorController();
		}

		private void ExitJumpState()
		{
			_animator.SetBool(_isJumpingAnimHash, false);
		}

		#endregion

		#region Fall State

		private void EnterFallState()
		{
			ResetFallingDuration();
			_velocity.y = 0f;

			DeactivateCrouch();
			_isSliding = false;
		}

		private void UpdateFallState(float delta)
		{
			GroundedCheck();

			CalculateRotationalAdditives(false, _enableHeadTurn, _enableBodyTurn);

			CalculateMoveDirection();
			FaceMoveDirection();

			ApplyGravity();
			Move(delta);
			UpdateAnimatorController();

			if (_controller.isGrounded)
				SwitchState(AnimationState.Locomotion);

			UpdateFallingDuration(delta);
		}

		private void UpdateFallingDuration(float delta)
		{
			_fallingDuration = delta - _fallStartTime;
		}

		private void ResetFallingDuration()
		{
			_fallStartTime = Time.time;
			_fallingDuration = 0f;
		}

		#endregion

		#region Crouch State

		private void EnterCrouchState()
		{
			_input.onJumpPerformed += CrouchToJumpState;
		}

		private void CeilingHeightCheck()
		{
			float rayDistance = Mathf.Infinity;
			float minimumStandingHeight = _capsuleStandingHeight - _frontRayPos.localPosition.y;

			Vector3 midpoint = new Vector3(_controller.transform.position.x, _controller.transform.position.y + _frontRayPos.localPosition.y, _controller.transform.position.z);
			if (Physics.Raycast(midpoint, _controller.transform.TransformDirection(Vector3.up), out RaycastHit ceilingHit, rayDistance, _groundLayerMask))
			{
				_cannotStandUp = ceilingHit.distance < minimumStandingHeight;
			}
			else
			{
				_cannotStandUp = false;
			}
		}

		private void UpdateCrouchState(float delta)
		{
			GroundedCheck();
			if (!_isGrounded)
			{
				DeactivateCrouch();
				CapsuleCrouchingSize(false);
				SwitchState(AnimationState.Fall);
			}

			CeilingHeightCheck();

			if (!_crouchKeyPressed && !_cannotStandUp)
			{
				DeactivateCrouch();
				SwitchToLocomotionState();
			}

			if (!_isCrouching)
			{
				CapsuleCrouchingSize(false);
				SwitchToLocomotionState();
			}

			CheckEnableTurns();
			CheckEnableLean();

			CalculateRotationalAdditives(false, _enableHeadTurn, false);

			CalculateMoveDirection();
			CheckIfStarting();
			CheckIfStopped();

			FaceMoveDirection();
			Move(delta);
			UpdateAnimatorController();
		}

		/// <summary>
		///     Performs the required actions upon exiting the crouch state.
		/// </summary>
		private void ExitCrouchState()
		{
			_input.onJumpPerformed -= CrouchToJumpState;
		}

		/// <summary>
		///     Moves from the crouch state to the jump state.
		/// </summary>
		private void CrouchToJumpState()
		{
			if (!_cannotStandUp)
			{
				DeactivateCrouch();
				SwitchState(AnimationState.Jump);
			}
		}

		/// <summary>
		///     Moves from the crouch state to the locomotion state.
		/// </summary>
		private void SwitchToLocomotionState()
		{
			DeactivateCrouch();
			SwitchState(AnimationState.Locomotion);
		}

		#endregion

		private void EnterBaseState()
		{
			_previousRotation = _controller.transform.forward;
		}

		private void CalculateInput()
		{
			if (_input.MovementInputDetected)
			{
				if (_input.MovementInputDuration == 0)
				{
					_movementInputTapped = true;
				}
				else if (_input.MovementInputDuration > 0 && _input.MovementInputDuration < _buttonHoldThreshold)
				{
					_movementInputTapped = false;
					_movementInputPressed = true;
					_movementInputHeld = false;
				}
				else
				{
					_movementInputTapped = false;
					_movementInputPressed = false;
					_movementInputHeld = true;
				}

				_input.MovementInputDuration += Time.deltaTime;
			}
			else
			{
				_input.MovementInputDuration = 0;
				_movementInputTapped = false;
				_movementInputPressed = false;
				_movementInputHeld = false;
			}

			_moveDirection = (_camera.GetCameraForwardZeroedYNormalised() * _input.MoveComposite.y)
			                 + (_camera.GetCameraRightZeroedYNormalised() * _input.MoveComposite.x);
		}
	}
}