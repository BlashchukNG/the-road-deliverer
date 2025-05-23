using Infrastructure.Roots.AppRoot.Services.UserInput;
using Logic.Characters.Player;
using Logic.UserCamera;
using UnityEngine;

namespace Logic.Characters.Base.Locomotions
{
	public sealed class References
	{
		public PlayerBinder View { get; }
		public IUserInputService Input { get; }
		public CameraController Camera { get; }
		public CharacterController Controller { get; }
		public Animator Animator { get; }
		public Transform FrontRayPos { get; }
		public Transform RearRayPos { get; }

		public References(PlayerBinder view, CameraController camera, IUserInputService input)
		{
			View = view;
			Input = input;
			Camera = camera;
			Controller = view.CharacterController;
			Animator = view.Animator;

			FrontRayPos = view.FrontRayPos;
			RearRayPos = view.RearRayPos;
		}
	}
}