namespace Logic.Characters.Base
{
	public interface ICharacterLocomotion
	{
		void Move(float horizontalAxis, float verticalAxis, float delta);
		void Jump();
		void Run(bool isRunning);
		void Crouch(bool isCrouching);
	}
}