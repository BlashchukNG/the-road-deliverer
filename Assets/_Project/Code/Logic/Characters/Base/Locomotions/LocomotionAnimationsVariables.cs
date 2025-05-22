using UnityEngine;

namespace Logic.Characters.Base.Locomotions
{
	public sealed class LocomotionAnimationsVariables
	{
		public readonly int movementInputTappedHash = Animator.StringToHash("MovementInputTapped");
		public readonly int movementInputPressedHash = Animator.StringToHash("MovementInputPressed");
		public readonly int movementInputHeldHash = Animator.StringToHash("MovementInputHeld");
		public readonly int shuffleDirectionXHash = Animator.StringToHash("ShuffleDirectionX");
		public readonly int shuffleDirectionZHash = Animator.StringToHash("ShuffleDirectionZ");
		public readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
		public readonly int currentGaitHash = Animator.StringToHash("CurrentGait");
		public readonly int isJumpingAnimHash = Animator.StringToHash("IsJumping");
		public readonly int fallingDurationHash = Animator.StringToHash("FallingDuration");
		public readonly int inclineAngleHash = Animator.StringToHash("InclineAngle");
		public readonly int strafeDirectionXHash = Animator.StringToHash("StrafeDirectionX");
		public readonly int strafeDirectionZHash = Animator.StringToHash("StrafeDirectionZ");
		public readonly int forwardStrafeHash = Animator.StringToHash("ForwardStrafe");
		public readonly int cameraRotationOffsetHash = Animator.StringToHash("CameraRotationOffset");
		public readonly int isStrafingHash = Animator.StringToHash("IsStrafing");
		public readonly int isTurningInPlaceHash = Animator.StringToHash("IsTurningInPlace");
		public readonly int isCrouchingHash = Animator.StringToHash("IsCrouching");
		public readonly int isWalkingHash = Animator.StringToHash("IsWalking");
		public readonly int isStoppedHash = Animator.StringToHash("IsStopped");
		public readonly int isStartingHash = Animator.StringToHash("IsStarting");
		public readonly int isGroundedHash = Animator.StringToHash("IsGrounded");
		public readonly int leanValueHash = Animator.StringToHash("LeanValue");
		public readonly int headLookXHash = Animator.StringToHash("HeadLookX");
		public readonly int headLookYHash = Animator.StringToHash("HeadLookY");
		public readonly int bodyLookXHash = Animator.StringToHash("BodyLookX");
		public readonly int bodyLookYHash = Animator.StringToHash("BodyLookY");
		public readonly int locomotionStartDirectionHash = Animator.StringToHash("LocomotionStartDirection");
	}
}