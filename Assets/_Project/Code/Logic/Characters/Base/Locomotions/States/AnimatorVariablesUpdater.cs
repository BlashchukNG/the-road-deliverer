namespace Logic.Characters.Base.Locomotions.States
{
	public sealed class AnimatorVariablesUpdater
	{
		private readonly CharacterModel _model;

		public AnimatorVariablesUpdater(CharacterModel model)
		{
			_model = model;
		}

		public void UpdateAnimatorController()
		{
			_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.leanValueHash, _model.LocomotionRuntimeData.leanValue);
			_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.headLookXHash, _model.LocomotionRuntimeData.headLookX);
			_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.headLookYHash, _model.LocomotionRuntimeData.headLookY);
			_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.bodyLookXHash, _model.LocomotionRuntimeData.bodyLookX);
			_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.bodyLookYHash, _model.LocomotionRuntimeData.bodyLookY);
			_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.isStrafingHash, _model.LocomotionRuntimeData.isStrafing ? 1.0f : 0.0f);
			_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.inclineAngleHash, _model.LocomotionRuntimeData.inclineAngle);
			_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.moveSpeedHash, _model.LocomotionRuntimeData.speed2D);
			_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.strafeDirectionXHash, _model.LocomotionRuntimeData.strafeDirectionX);
			_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.strafeDirectionZHash, _model.LocomotionRuntimeData.strafeDirectionZ);
			_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.forwardStrafeHash, _model.LocomotionSettings.forwardStrafe);
			_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.cameraRotationOffsetHash, _model.LocomotionSettings.cameraRotationOffset);
			_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.shuffleDirectionXHash, _model.LocomotionSettings.shuffleDirectionX);
			_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.shuffleDirectionZHash, _model.LocomotionSettings.shuffleDirectionZ);
			_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.fallingDurationHash, _model.LocomotionRuntimeData.fallingDuration);
			_model.LocomotionReferences.Animator.SetFloat(_model.LocomotionAnimationsVariables.locomotionStartDirectionHash, _model.LocomotionRuntimeData.locomotionStartDirection);
			_model.LocomotionReferences.Animator.SetInteger(_model.LocomotionAnimationsVariables.currentGaitHash, (int)_model.LocomotionRuntimeData.currentGait);
			_model.LocomotionReferences.Animator.SetBool(_model.LocomotionAnimationsVariables.movementInputHeldHash, _model.LocomotionRuntimeData.movementInputHeld);
			_model.LocomotionReferences.Animator.SetBool(_model.LocomotionAnimationsVariables.movementInputPressedHash, _model.LocomotionRuntimeData.movementInputPressed);
			_model.LocomotionReferences.Animator.SetBool(_model.LocomotionAnimationsVariables.movementInputTappedHash, _model.LocomotionRuntimeData.movementInputTapped);
			_model.LocomotionReferences.Animator.SetBool(_model.LocomotionAnimationsVariables.isTurningInPlaceHash, _model.LocomotionRuntimeData.isTurningInPlace);
			_model.LocomotionReferences.Animator.SetBool(_model.LocomotionAnimationsVariables.isCrouchingHash, _model.LocomotionRuntimeData.isCrouching);
			_model.LocomotionReferences.Animator.SetBool(_model.LocomotionAnimationsVariables.isGroundedHash, _model.LocomotionRuntimeData.isGrounded);
			_model.LocomotionReferences.Animator.SetBool(_model.LocomotionAnimationsVariables.isWalkingHash, _model.LocomotionRuntimeData.isWalking);
			_model.LocomotionReferences.Animator.SetBool(_model.LocomotionAnimationsVariables.isStoppedHash, _model.LocomotionRuntimeData.isStopped);
		}
	}
}