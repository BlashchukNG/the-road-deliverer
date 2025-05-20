using Infrastructure.State.Entities.Player.Characteristics.Proxy;

namespace Logic.Characters.Base
{
	public sealed class CharacterModel
	{
		public CharacterCharacteristics Characteristics { get; }

		public CharacterModel(CharacteristicsDataProxy characteristicsData)
		{
			Characteristics = new CharacterCharacteristics(characteristicsData);
		}
	}
}