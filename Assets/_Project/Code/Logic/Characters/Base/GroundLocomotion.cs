using Constants;
using UnityEngine;

namespace Logic.Characters.Base
{
	public sealed class GroundLocomotion : ICharacterLocomotion
	{
		private readonly ICharacterView _view;
		private readonly CharacterModel _model;
		private readonly Transform _camera;

		private Vector3 _velocity;
		private float _targetSpeed;
		private float _standingHeight = 1.8f;
		private float _crouchingHeight = 1.0f;
		private bool _isCrouching;
		private bool _isRunning;
		private bool _isJumping;

		public GroundLocomotion(ICharacterView view, CharacterModel model, Transform camera)
		{
			_view = view;
			_model = model;
			_camera = camera;

			_targetSpeed = model.Characteristics.MoveSpeed;
		}

		public void Move(float horizontalAxis, float verticalAxis, float delta)
		{
			var forward = Vector3.Scale(_camera.forward, new Vector3(1, 0, 1)).normalized;
			var right = Vector3.Scale(_camera.right, new Vector3(1, 0, 1)).normalized;

			var direction = forward * verticalAxis + right * horizontalAxis;
			direction.Normalize();
			direction *= _targetSpeed;

			if (_isJumping)
			{
				_velocity.y = Mathf.Sqrt(_model.Characteristics.JumpHeight * CharacteristicConstants.GRAVITY_COEFFICIENT * CharacteristicConstants.GRAVITY);
				_isJumping = false;
			}

			if (_view.CharacterController.isGrounded)
			{
				if (_velocity.y < 0) _velocity.y = CharacteristicConstants.GRAVITY_COEFFICIENT;

				_velocity.x = direction.x;
				_velocity.z = direction.z;
			}
			else
			{
				var dampedVelocity = _velocity;
				dampedVelocity.y = 0;
				dampedVelocity = Vector3.ClampMagnitude(dampedVelocity, Mathf.Max(0, dampedVelocity.magnitude - CharacteristicConstants.LINEAR_DAMPING * delta / 2));

				_velocity.x = dampedVelocity.x;
				_velocity.z = dampedVelocity.z;
			}

			_velocity.y += CharacteristicConstants.GRAVITY * delta;

			_view.CharacterController.Move(_velocity * delta);

			Turn(horizontalAxis, verticalAxis, direction, delta);
		}

		private void Turn(float horizontalAxis, float verticalAxis, Vector3 direction, float delta)
		{
			if (Mathf.Abs(horizontalAxis) > 0 || Mathf.Abs(verticalAxis) > 0)
			{
				var lookDirection = direction.normalized;
				lookDirection.y = 0;

				var rotation = Quaternion.LookRotation(lookDirection);
				_view.Transform.rotation = Quaternion.Slerp(_view.Transform.rotation, rotation, delta * 30);
			}
		}


		public void Jump()
		{
			if (!_view.CharacterController.isGrounded) return;

			_isJumping = true;
		}

		public void Run(bool isRunning)
		{
			if (_isCrouching) return;

			_isRunning = isRunning;
			_targetSpeed = !_isRunning ? _model.Characteristics.MoveSpeed : _model.Characteristics.MoveSpeed * 1.5f;
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