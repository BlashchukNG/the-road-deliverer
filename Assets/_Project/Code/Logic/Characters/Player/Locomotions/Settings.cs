using System;
using UnityEngine;

namespace Logic.Characters.Base.Locomotions
{
	[Serializable]
	public sealed class Settings : ICloneable
	{
		public readonly float ANIMATION_DAMP_TIME = 5f;
		public readonly float STRAFE_DIRECTION_DAMP_TIME = 20f;
		
		[Header("Locomotion Settings")]
		public bool alwaysStrafe = true;
		public float speedChangeDamping = 10f;
		public float rotationSmoothing = 10f;
		public float cameraRotationOffset;
		public float buttonHoldThreshold = 0.15f;
		public float shuffleDirectionX;
		public float shuffleDirectionZ;

		[Header("Capsule Settings")]
		public float capsuleStandingHeight = 1.8f;
		public float capsuleStandingCentre = 0.93f;
		public float capsuleCrouchingHeight = 1.2f;
		public float capsuleCrouchingCentre = 0.6f;

		[Header("Ground Settings")]
		public LayerMask groundLayerMask;
		public float groundedOffset = -0.14f;

		[Header("In-Air Settings")]
		public float jumpForce = 10f;
		public float gravityMultiplier = 2f;

		[Header("Head Look Settings")]
		public bool enableHeadTurn = true;
		public AnimationCurve headLookXCurve;

		[Header("Body Look Settings")]
		public bool enableBodyTurn = true;
		public AnimationCurve bodyLookXCurve;

		[Header("Lean Settings")]
		public bool enableLean = true;
		public AnimationCurve leanCurve;
		
		[Header("Player Strafing")]
		public float forwardStrafeMinThreshold = -55.0f;
		public float forwardStrafeMaxThreshold = 125.0f;
		public float forwardStrafe = 1f;

		public object Clone() => MemberwiseClone();
	}
}