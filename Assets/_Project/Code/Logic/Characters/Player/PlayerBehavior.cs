using Infrastructure.Roots.AppRoot.Services.UserInput;
using Infrastructure.State.Entities.Player;
using Logic.Characters.Player.Locomotions;
using Logic.UserCamera;

namespace Logic.Characters.Player
{
	public sealed class PlayerBehavior
	{
		public PlayerEntityProxy Entity { get; }
		public CharacterCharacteristics Characteristics { get; }
		public References References { get; private set; }
		public Settings Settings { get; }
		public RuntimeData RuntimeData { get; }
		public AnimationsVariables AnimationsVariables { get; }
		

		private readonly IUserInputService _input;
		

		public PlayerBehavior(IUserInputService input, PlayerEntityProxy entity, Settings settings)
		{
			_input = input;
			Entity = entity;
			Characteristics = new CharacterCharacteristics(entity.CharacteristicsData);
			Settings = settings.Clone() as Settings;
			RuntimeData = new();
			AnimationsVariables = new();
		}

		public void AttachReferences(PlayerBinder playerBinder, CameraController camera)
		{
			References = new References(playerBinder, camera, _input);
		}
	}
}