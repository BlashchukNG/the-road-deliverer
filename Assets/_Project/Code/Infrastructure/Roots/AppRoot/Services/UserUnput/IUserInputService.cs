using System;
using UnityEngine;

namespace Infrastructure.Roots.AppRoot.Services.UserUnput
{
	public interface IUserInputService
	{
		public GameObject GameObject { get; }
		public event Action<Vector2> OnMouseButtonDown;
		public float HorizontalAxis();
		public float VerticalAxis();
		public Vector2 MousePosition();
	}
}