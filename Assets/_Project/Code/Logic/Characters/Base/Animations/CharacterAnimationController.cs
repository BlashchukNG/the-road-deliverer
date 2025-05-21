using Logic.Characters.Base.Locomotion;
using UnityEngine;

namespace Logic.Characters.Base.Animations
{
	public class CharacterAnimationController : ICharacterAnimationController
	{
		private readonly int _hashVelocityX = Animator.StringToHash("velocity_x");
		private readonly int _hashVelocityZ = Animator.StringToHash("velocity_z");
		private readonly int _hashIsCroaching = Animator.StringToHash("is_croaching");

		private readonly CharacterModel _model;
		private readonly ICharacterLocomotion _locomotion;
		private readonly Animator _animator;
		private readonly CharacterController _characterController;
		private float _velocityX;
		private float _velocityZ;

		public CharacterAnimationController(ICharacterView view, CharacterModel model, ICharacterLocomotion locomotion)
		{
			_model = model;
			_locomotion = locomotion;
			_animator = view.Animator;
			_characterController = view.CharacterController;
		}


		public void Update(float delta)
		{
			_velocityX = Mathf.Lerp(_velocityX, _locomotion.RelativityDirection.x, 8 * delta);
			_velocityZ = Mathf.Lerp(_velocityZ, _locomotion.RelativityDirection.z, 8 * delta);
			
			_animator.SetFloat(_hashVelocityX, _velocityX);
			_animator.SetFloat(_hashVelocityZ, _velocityZ);
		}
	}
}