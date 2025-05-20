using System;
using Infrastructure.Roots.AppRoot.Services.Updater;
using UnityEngine;

namespace Infrastructure.Roots.AppRoot.Services.UserUnput
{
	public interface IUserInputService : ITick
	{
		event Action<Vector2> OnMouseButtonDown;
		event Action<bool> OnRun;
		event Action<bool> OnCrouch;
		event Action OnJump;


		float HorizontalMouseAxis();
		float GetMouseScrollWheel();
		float HorizontalAxis();
		float VerticalAxis();
		Vector2 MousePosition();
	}
}