using System;
using Infrastructure.Roots.AppRoot.Services.Updater;
using UnityEngine;

namespace Infrastructure.Roots.AppRoot.Services.UserUnput
{
	public class MobileUserInputService : IUserInputService, ITick
	{
		public event Action<Vector2> OnMouseButtonDown;
		public event Action<bool> OnRun;
		public event Action<bool> OnCrouch;
		public event Action OnJump;

		public float HorizontalAxis()
		{
			throw new NotImplementedException();
		}

		public float VerticalAxis()
		{
			throw new NotImplementedException();
		}

		public Vector2 MousePosition()
		{
			throw new NotImplementedException();
		}

		public void Tick(float delta)
		{
			throw new NotImplementedException();
		}
	}
}