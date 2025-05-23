using Infrastructure.Roots.AppRoot.Services.UserUnput;
using Infrastructure.State.Entities.Player;
using Infrastructure.State.Entities.Player.Characteristics.Proxy;
using Logic.Characters.Base.Locomotions;
using Logic.UserCamera;

namespace Logic.Characters.Base
{
	public sealed class CharacterModel
	{
		public PlayerEntityProxy Entity { get; }
		public CharacterCharacteristics Characteristics { get; }
		public LocomotionReferences LocomotionReferences { get; }
		public LocomotionSettings LocomotionSettings { get; }
		public LocomotionRuntimeData LocomotionRuntimeData { get; }
		public LocomotionAnimationsVariables LocomotionAnimationsVariables { get; }

		public CharacterModel(ICharacterView view, CameraController camera, IUserInputService input, PlayerEntityProxy entity, LocomotionSettings settings)
		{
			Entity = entity;
			Characteristics = new CharacterCharacteristics(entity.CharacteristicsData);
			LocomotionSettings = settings.Clone() as LocomotionSettings;
			LocomotionReferences = new LocomotionReferences(view, camera, input);
			LocomotionRuntimeData = new();
			LocomotionAnimationsVariables = new();
		}
	}
}