using System;
using UnityEngine;

namespace Infrastructure.Roots.AppRoot.Services.UserUnput
{
	public class MobileUserInputService : MonoBehaviour, IUserInputService
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
	}
}