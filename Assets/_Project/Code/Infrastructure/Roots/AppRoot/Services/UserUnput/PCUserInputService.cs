using System;
using Infrastructure.Roots.AppRoot.Services.Updater;
using UnityEngine;

namespace Infrastructure.Roots.AppRoot.Services.UserUnput
{
	public class PCUserInputService : IUserInputService, ITick
	{
		public event Action<Vector2> OnMouseButtonDown = vector2 => { };
		public event Action<bool> OnRun = b => { };
		public event Action<bool> OnCrouch = b => { };
		public event Action OnJump = () => { };


		public float HorizontalMouseAxis() => Input.GetMouseButton(1) ? Input.GetAxis("Mouse X") : 0;
		public float GetMouseScrollWheel() => Input.GetAxis("Mouse ScrollWheel");

		public float HorizontalAxis() => Input.GetAxis("Horizontal");
		public float VerticalAxis() => Input.GetAxis("Vertical");
		public Vector2 MousePosition() => Input.mousePosition;

		public void Tick(float delta)
		{
			if (Input.GetMouseButtonDown(0)) OnMouseButtonDown.Invoke(Input.mousePosition);

			if (Input.GetKeyDown(KeyCode.Space)) OnJump.Invoke();

			if (Input.GetKeyDown(KeyCode.LeftShift)) OnRun.Invoke(true);
			if (Input.GetKeyUp(KeyCode.LeftShift)) OnRun.Invoke(false);

			if (Input.GetKeyDown(KeyCode.LeftControl)) OnCrouch.Invoke(true);
			if (Input.GetKeyUp(KeyCode.LeftControl)) OnCrouch.Invoke(false);
		}
	}
}