using Infrastructure.DI;
using Infrastructure.Roots.AppRoot.Services.Updater;
using Infrastructure.Roots.AppRoot.Services.UserUnput;
using Infrastructure.State;
using Infrastructure.State.Entities.UserCamera;
using UnityEngine;

namespace Logic.UserCamera
{
	public sealed class CameraController : MonoBehaviour, ILateTick
	{
		public Camera Camera => _camera;

		[SerializeField] private Camera _camera;
		[SerializeField] private Transform _followTarget;

		private IUserInputService _input;
		private CameraSettingsDataProxy _settings;
		private float _targetDistance;


		public CameraController SetInput(DIContainer diContainer)
		{
			_input = diContainer.Resolve<IUserInputService>();
			_settings = diContainer.Resolve<IGameStateProvider>().GameState.CameraSettings;
			return this;
		}

		public CameraController SetFollowTarget(Transform target)
		{
			_followTarget = target;
			return this;
		}
		
		public void LateTick(float delta)
		{
			HandleRotation(delta);
			HandleZoom(delta);
			UpdatePosition(delta);
		}

		private void HandleRotation(float delta)
		{
			var axis = _input.HorizontalMouseAxis * _settings.RotationSpeed.Value;
			_settings.Yaw.Value += axis * delta;

			if (_settings.Yaw.Value < 0)
				_settings.Yaw.Value += 360;
			else if (_settings.Yaw.Value > 360)
				_settings.Yaw.Value -= 360;
		}

		private void HandleZoom(float delta)
		{
			float scrollInput = _input.GetMouseScrollWheel * _settings.ZoomSpeed.Value * delta;

			_targetDistance = _settings.Distance.Value;
			_targetDistance -= scrollInput;
			_targetDistance = Mathf.Clamp(_targetDistance, _settings.MinDistance.Value, _settings.MaxDistance.Value);

			_settings.Distance.Value = Mathf.Lerp(_settings.Distance.Value, _targetDistance, delta * _settings.ZoomSmooth.Value);
		}

		void UpdatePosition(float delta)
		{
			var rotation = Quaternion.Euler(0, _settings.Yaw.Value, 0);
			transform.position = Vector3.Lerp(transform.position, _followTarget.position + rotation * (_settings.Offset.Value.normalized * _settings.Distance.Value), 50f * delta);
			transform.LookAt(_followTarget.position + Vector3.up * (_settings.Offset.Value.y * 0.1f));
		}

		public Vector3 GetCameraForwardZeroedYNormalised() => GetCameraForwardZeroedY().normalized;
		public Vector3 GetCameraRightZeroedYNormalised() => new Vector3(transform.right.x, 0, transform.right.z);

		public Vector3 GetCameraForward() => transform.forward;
		public Vector3 GetCameraForwardZeroedY() => new(transform.forward.x, 0, transform.forward.z);
		
		public float GetCameraTiltX() => transform.eulerAngles.x;
	}
}