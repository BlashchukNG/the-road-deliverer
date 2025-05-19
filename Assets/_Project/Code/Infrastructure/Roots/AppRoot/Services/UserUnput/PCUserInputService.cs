using System;
using UnityEngine;

namespace Infrastructure.Roots.AppRoot.Services.UserUnput
{
	public class PCUserInputService : MonoBehaviour, IUserInputService
	{
		public GameObject GameObject => gameObject;

		public event Action<Vector2> OnMouseButtonDown = vector2 => { };

		public float HorizontalAxis() => Input.GetAxis("Horizontal");
		public float VerticalAxis() => Input.GetAxis("Vertical");
		public Vector2 MousePosition() => Input.mousePosition;

		private void Update()
		{
			if (Input.GetMouseButtonDown(0)) OnMouseButtonDown.Invoke(Input.mousePosition);
		}
	}
}