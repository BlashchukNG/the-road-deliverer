using System;
using Infrastructure.Roots.AppRoot.Services.Updater;
using UnityEngine;

namespace Infrastructure.Roots.AppRoot.Services.UserUnput
{
	public interface IUserInputService : ITick
	{
		public event Action<Vector2> OnMouseButtonDown;
		public float HorizontalAxis();
		public float VerticalAxis();
		public Vector2 MousePosition();
	}
}