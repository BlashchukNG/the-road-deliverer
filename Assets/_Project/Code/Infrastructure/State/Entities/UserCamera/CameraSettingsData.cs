using System;
using UnityEngine;

namespace Infrastructure.State.Entities.UserCamera
{
	[Serializable]
	public sealed class CameraSettingsData
	{
		public Vector3 offset;
		public float distance;
		public float minDistance;
		public float maxDistance;
		public float rotationSpeed;
		public float yaw;
		public float zoomSpeed;
	}
}