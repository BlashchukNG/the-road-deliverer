using UnityEngine;

namespace UserCamera
{
	public sealed class CameraController : MonoBehaviour
	{
		public Camera Camera => _camera;

		[SerializeField] private Camera _camera;
	}
}