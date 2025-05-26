using UnityEngine;

namespace Logic.UserCamera
{
	public sealed class HideObject : MonoBehaviour
	{
		[SerializeField] private MeshRenderer[] _renderers;

		public void Hide()
		{
			foreach (var r in _renderers)
				r.enabled = false;
		}

		public void Show()
		{
			foreach (var r in _renderers)
				r.enabled = true;	
		}
	}
}