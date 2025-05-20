using Constants;
using UnityEngine;

namespace Logic.Characters.Base
{
	public sealed class GroundLocomotion : ICharacterLocomotion
	{
		private readonly ICharacterView _view;
		private readonly CharacterModel _model;

		private float _targetSpeed;
		private float _standingHeight = 1.8f;
		private float _crouchingHeight = 1.0f;
		private bool _isCrouching;
		private bool _isRunning;
		private Vector3 _velocity;
		private Vector3 _movementDirection;

		public GroundLocomotion(ICharacterView view, CharacterModel model)
		{
			_view = view;
			_model = model;

			_targetSpeed = model.Characteristics.MoveSpeed;
		}

		public void Move(float horizontalAxis, float verticalAxis, float delta)
		{
			_movementDirection.x = horizontalAxis;
			_movementDirection.z = verticalAxis;
			_movementDirection.Normalize();
			
			_view.CharacterController.Move(_movementDirection * _targetSpeed * delta);
		}

		

		public void Jump()
		{
			if (_view.CharacterController.isGrounded)
			{
				
			}
		}

		public void Run(bool isRunning)
		{
			if (_isCrouching) return;

			_isRunning = isRunning;
			_targetSpeed = !_isRunning ? _model.Characteristics.MoveSpeed : _model.Characteristics.MoveSpeed * 2f;
		}

		public void Crouch(bool isCrouching)
		{
			_isCrouching = isCrouching;
			_targetSpeed = !_isCrouching ? _model.Characteristics.MoveSpeed : _model.Characteristics.MoveSpeed / 2f;
			UpdateColliderHeight();
		}

		private void UpdateColliderHeight()
		{
			var targetHeight = _isCrouching ? _crouchingHeight : _standingHeight;
			var center = _view.CharacterController.center;
			center.y = targetHeight / 2f;

			_view.CharacterController.center = center;
			_view.CharacterController.height = targetHeight;
		}
	}
}