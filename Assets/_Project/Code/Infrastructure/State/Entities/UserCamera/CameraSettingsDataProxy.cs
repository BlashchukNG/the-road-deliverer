using R3;
using UnityEngine;

namespace Infrastructure.State.Entities.UserCamera
{
	public sealed class CameraSettingsDataProxy
	{
		public ReactiveProperty<Vector3> Offset { get; }
		public ReactiveProperty<float> Distance { get; }
		public ReactiveProperty<float> MinDistance { get; }
		public ReactiveProperty<float> MaxDistance { get; }
		public ReactiveProperty<float> RotationSpeed { get; }
		public ReactiveProperty<float> Yaw { get; }
		public ReactiveProperty<float> ZoomSpeed { get; }

		
		public CameraSettingsDataProxy(CameraSettingsData data)
		{
			Offset = new ReactiveProperty<Vector3>(data.offset);
			Distance = new ReactiveProperty<float>(data.distance);
			MinDistance = new ReactiveProperty<float>(data.minDistance);
			MaxDistance = new ReactiveProperty<float>(data.maxDistance);
			RotationSpeed = new ReactiveProperty<float>(data.rotationSpeed);
			Yaw = new ReactiveProperty<float>(data.yaw);
			ZoomSpeed = new ReactiveProperty<float>(data.zoomSpeed);
			
			Offset.Skip(1).Subscribe(value => data.offset = value);
			Distance.Skip(1).Subscribe(value => data.distance = value);
			MinDistance.Skip(1).Subscribe(value => data.minDistance = value);
			MaxDistance.Skip(1).Subscribe(value => data.maxDistance = value);
			RotationSpeed.Skip(1).Subscribe(value => data.rotationSpeed = value);
			Yaw.Skip(1).Subscribe(value => data.yaw = value);
			ZoomSpeed.Skip(1).Subscribe(value => data.zoomSpeed = value);
		}
	}
}