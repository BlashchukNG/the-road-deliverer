namespace Logic.Characters.Base.Locomotions.States
{
	public sealed class AnimatorVariablesUpdater
	{
		private readonly PlayerBehavior _model;

		public AnimatorVariablesUpdater(PlayerBehavior model)
		{
			_model = model;
		}

		public void UpdateAnimatorController()
		{
			_model.References.Animator.SetFloat(_model.AnimationsVariables.leanValueHash, _model.RuntimeData.leanValue);
			_model.References.Animator.SetFloat(_model.AnimationsVariables.headLookXHash, _model.RuntimeData.headLookX);
			_model.References.Animator.SetFloat(_model.AnimationsVariables.headLookYHash, _model.RuntimeData.headLookY);
			_model.References.Animator.SetFloat(_model.AnimationsVariables.bodyLookXHash, _model.RuntimeData.bodyLookX);
			_model.References.Animator.SetFloat(_model.AnimationsVariables.bodyLookYHash, _model.RuntimeData.bodyLookY);
			_model.References.Animator.SetFloat(_model.AnimationsVariables.isStrafingHash, _model.RuntimeData.isStrafing ? 1.0f : 0.0f);
			_model.References.Animator.SetFloat(_model.AnimationsVariables.inclineAngleHash, _model.RuntimeData.inclineAngle);
			_model.References.Animator.SetFloat(_model.AnimationsVariables.moveSpeedHash, _model.RuntimeData.speed2D);
			_model.References.Animator.SetFloat(_model.AnimationsVariables.strafeDirectionXHash, _model.RuntimeData.strafeDirectionX);
			_model.References.Animator.SetFloat(_model.AnimationsVariables.strafeDirectionZHash, _model.RuntimeData.strafeDirectionZ);
			_model.References.Animator.SetFloat(_model.AnimationsVariables.forwardStrafeHash, _model.Settings.forwardStrafe);
			_model.References.Animator.SetFloat(_model.AnimationsVariables.cameraRotationOffsetHash, _model.Settings.cameraRotationOffset);
			_model.References.Animator.SetFloat(_model.AnimationsVariables.shuffleDirectionXHash, _model.Settings.shuffleDirectionX);
			_model.References.Animator.SetFloat(_model.AnimationsVariables.shuffleDirectionZHash, _model.Settings.shuffleDirectionZ);
			_model.References.Animator.SetFloat(_model.AnimationsVariables.fallingDurationHash, _model.RuntimeData.fallingDuration);
			_model.References.Animator.SetFloat(_model.AnimationsVariables.locomotionStartDirectionHash, _model.RuntimeData.locomotionStartDirection);
			_model.References.Animator.SetInteger(_model.AnimationsVariables.currentGaitHash, (int)_model.RuntimeData.currentGait);
			_model.References.Animator.SetBool(_model.AnimationsVariables.movementInputHeldHash, _model.RuntimeData.movementInputHeld);
			_model.References.Animator.SetBool(_model.AnimationsVariables.movementInputPressedHash, _model.RuntimeData.movementInputPressed);
			_model.References.Animator.SetBool(_model.AnimationsVariables.movementInputTappedHash, _model.RuntimeData.movementInputTapped);
			_model.References.Animator.SetBool(_model.AnimationsVariables.isTurningInPlaceHash, _model.RuntimeData.isTurningInPlace);
			_model.References.Animator.SetBool(_model.AnimationsVariables.isCrouchingHash, _model.RuntimeData.isCrouching);
			_model.References.Animator.SetBool(_model.AnimationsVariables.isGroundedHash, _model.RuntimeData.isGrounded);
			_model.References.Animator.SetBool(_model.AnimationsVariables.isWalkingHash, _model.RuntimeData.isWalking);
			_model.References.Animator.SetBool(_model.AnimationsVariables.isStoppedHash, _model.RuntimeData.isStopped);
		}
	}
}