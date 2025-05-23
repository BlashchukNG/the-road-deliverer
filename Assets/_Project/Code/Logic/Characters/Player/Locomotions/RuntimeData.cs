using UnityEngine;

namespace Logic.Characters.Player.Locomotions
{
	public sealed class RuntimeData
	{
		public float inclineAngle;
		
		public float fallingDuration;
		
		public float headLookDelay;
		public float headLookX;
		public float headLookY;
		
		public float bodyLookDelay;
		public float bodyLookX;
		public float bodyLookY;
		
		public float leanDelay;
		public float leanValue;
		public float leansHeadLooksDelay;
		public bool animationClipEnd;
		
		public AnimationState currentState = AnimationState.Base;
		public GaitState currentGait;
		public Vector3 currentRotation;
		public Vector3 moveDirection;
		public Vector3 previousRotation;
		public Vector3 velocity;
		public float currentMaxSpeed;
		public float locomotionStartDirection;
		public float locomotionStartTimer;
		public float lookingAngle;
		public float newDirectionDifferenceAngle;
		public float speed2D;
		public float strafeAngle;
		public float strafeDirectionX;
		public float strafeDirectionZ;
		public bool cannotStandUp;
		public bool crouchKeyPressed;
		public bool isCrouching;
		public bool isGrounded = true;
		public bool isSliding;
		public bool isSprinting;
		public bool isStarting;
		public bool isStopped = true;
		public bool isStrafing;
		public bool isTurningInPlace;
		public bool isWalking;
		public bool movementInputHeld;
		public bool movementInputPressed;
		public bool movementInputTapped;
		
		public Vector3 cameraForward;
		public Vector3 targetVelocity;
		public float targetMaxSpeed;
		public float fallStartTime;
		public float rotationRate;
		public float initialLeanValue;
		public float initialTurnValue;
	}
}