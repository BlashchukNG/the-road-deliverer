using Infrastructure.State.Entities.Player.Characteristics.Proxy;

namespace Logic.Characters.Base
{
	public sealed class CharacterModel
	{
		public CalculatedCharacteristics CalculatedCharacteristics { get; }

		public CharacterModel(CharacteristicsDataProxy characteristicsData)
		{
			CalculatedCharacteristics = new CalculatedCharacteristics(characteristicsData);
		}
	}
}