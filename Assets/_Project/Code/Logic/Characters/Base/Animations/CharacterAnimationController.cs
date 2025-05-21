using Logic.Characters.Base.Locomotion;
using UnityEngine;

namespace Logic.Characters.Base.Animations
{
	public class CharacterAnimationController : ICharacterAnimationController
	{
		private readonly int _hashVelocity = Animator.StringToHash("velocity");
		private readonly int _hashVelocityX = Animator.StringToHash("velocity_x");
		private readonly int _hashVelocityZ = Animator.StringToHash("velocity_z");
		private readonly int _hashIsCroaching = Animator.StringToHash("is_croaching");
		private readonly int _hashInAirTime = Animator.StringToHash("in_air_time");
		private readonly int _hashIsInAir = Animator.StringToHash("is_in_air");
		private readonly int _hashIsJumping = Animator.StringToHash("is_jumping");
		private readonly int _hashIsLanding = Animator.StringToHash("is_landing");

		private readonly ICharacterView _view;
		private readonly ICharacterLocomotion _locomotion;
		private readonly CharacterModel _model;
		private readonly Animator _animator;
		private readonly CharacterController _characterController;
		private float _velocityX;
		private float _velocityZ;

		public CharacterAnimationController(ICharacterView view, CharacterModel model, ICharacterLocomotion locomotion)
		{
			_view = view;
			_model = model;
			_locomotion = locomotion;
			_animator = view.Animator;
			_characterController = view.CharacterController;
		}


		public void Update(float delta)
		{
			_animator.SetBool(_hashIsCroaching, _locomotion.IsCroaching);
			_animator.SetBool(_hashIsLanding, _locomotion.IsLanding);
			_animator.SetBool(_hashIsInAir, _locomotion.IsInAir);
			
			_velocityX = Mathf.Lerp(_velocityX, Mathf.Clamp(_locomotion.RelativityDirection.x, -1f, 1f), 8 * delta);
			_velocityZ = Mathf.Lerp(_velocityZ, Mathf.Clamp(_locomotion.RelativityDirection.z, -1f, 1f), 8 * delta);

			_animator.SetFloat(_hashInAirTime, _locomotion.InAirTime);
			_animator.SetFloat(_hashVelocityX, _velocityX);
			_animator.SetFloat(_hashVelocityZ, _velocityZ);
			_animator.SetFloat(_hashVelocity, _locomotion.RelativityDirection.magnitude);
		}

		public void Jump()
		{
			if (!_characterController.isGrounded) return;
			
			_animator.SetTrigger(_hashIsJumping);
		}
	}
}