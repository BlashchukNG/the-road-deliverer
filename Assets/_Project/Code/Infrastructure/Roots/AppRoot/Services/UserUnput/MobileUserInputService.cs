using System;
using Infrastructure.Roots.AppRoot.Services.Updater;
using UnityEngine;

namespace Infrastructure.Roots.AppRoot.Services.UserUnput
{
	public class MobileUserInputService : IUserInputService, ITick
	{
		public GameObject GameObject { get; }
		public event Action<Vector2> OnMouseButtonDown;
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