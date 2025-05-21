using UnityEngine;

namespace Logic.Characters.Base.Locomotion
{
	public interface ICharacterLocomotion
	{
		Vector3 RelativityDirection { get; }
		void Move(Vector2 mousePosition, float horizontalAxis, float verticalAxis, float delta);
		void Jump();
		void Run(bool isRunning);
		void Crouch(bool isCrouching);
	}
}