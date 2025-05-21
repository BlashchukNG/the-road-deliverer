using Constants;
using Logic.UserCamera;
using UnityEngine;

namespace Logic.Characters.Base.Locomotion
{
	public sealed class GroundLocomotion : ICharacterLocomotion
	{
		private readonly ICharacterView _view;
		private readonly CharacterModel _model;
		private readonly CameraController _camera;

		private Vector3 _velocity;
		private Vector3 _direction;
		private float _targetSpeed;
		private float _standingHeight = 1.8f;
		private float _crouchingHeight = 1.0f;
		private bool _isCrouching;
		private bool _isRunning;
		private bool _isJumping;

		public Vector3 RelativityDirection => _view.Transform.InverseTransformDirection(new Vector3(_direction.x, 0, _direction.z).normalized * (_isRunning ? 1 : 0.5f));


		public GroundLocomotion(ICharacterView view, CharacterModel model, CameraController camera)
		{
			_view = view;
			_model = model;
			_camera = camera;

			_targetSpeed = model.Characteristics.MoveSpeed;
		}

		public void Move(Vector2 mousePosition, float horizontalAxis, float verticalAxis, float delta)
		{
			var forward = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
			var right = Vector3.Scale(_camera.transform.right, new Vector3(1, 0, 1)).normalized;

			_direction = forward * verticalAxis + right * horizontalAxis;
			_direction.Normalize();
			_direction *= _targetSpeed;

			if (_isJumping)
			{
				_velocity.y = Mathf.Sqrt(_model.Characteristics.JumpHeight * CharacteristicConstants.GRAVITY_COEFFICIENT * CharacteristicConstants.GRAVITY);
				_isJumping = false;
			}

			if (_view.CharacterController.isGrounded)
			{
				if (_velocity.y < 0) _velocity.y = CharacteristicConstants.GRAVITY_COEFFICIENT;

				_velocity.x = _direction.x;
				_velocity.z = _direction.z;
			}

			_velocity.y += CharacteristicConstants.GRAVITY * delta;


			Turn(mousePosition, delta);

			_view.CharacterController.Move(_velocity * delta);
		}

		private void Turn(Vector2 mousePosition, float delta)
		{
			var ray = _camera.Camera.ScreenPointToRay(mousePosition);

			if (Physics.Raycast(ray, out var hit, 300, LayerMask.GetMask("ground")))
			{
				var hitPosition = new Vector3(hit.point.x, _view.Transform.position.y, hit.point.z);
				var rotation = Quaternion.LookRotation(hitPosition - _view.Transform.position);
				_view.Transform.rotation = Quaternion.Slerp(_view.Transform.rotation, rotation, delta * 8f);
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
			_targetSpeed = !_isRunning ? _model.Characteristics.MoveSpeed : _model.Characteristics.MoveSpeed * CharacteristicConstants.MOD_RUN;
		}

		public void Crouch(bool isCrouching)
		{
			_isCrouching = isCrouching;
			_targetSpeed = !_isCrouching ? _model.Characteristics.MoveSpeed : _model.Characteristics.MoveSpeed * CharacteristicConstants.MOD_CROUCH;
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