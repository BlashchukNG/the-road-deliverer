using System;
using Infrastructure.Roots.AppRoot.Services.Updater;
using UnityEngine;

namespace Infrastructure.Roots.AppRoot.Services.UserUnput
{
	public class PCUserInputService : IUserInputService, ITick
	{
		public event Action<Vector2> OnMouseButtonDown = vector2 => { };

		public float HorizontalAxis() => Input.GetAxis("Horizontal");
		public float VerticalAxis() => Input.GetAxis("Vertical");
		public Vector2 MousePosition() => Input.mousePosition;

		public void Tick(float delta)
		{
			if (Input.GetMouseButtonDown(0)) OnMouseButtonDown.Invoke(Input.mousePosition);
		}
	}
}