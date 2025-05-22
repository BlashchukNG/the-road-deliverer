using Infrastructure.State.Entities.Player.Characteristics.Data;

namespace Infrastructure.State.Entities.Player.Characteristics.Proxy
{
	public sealed class PrimaryCharacteristicsProxy
	{
		public CharacteristicProxy Strength { get; }
		public CharacteristicProxy Dexterity { get; }
		public CharacteristicProxy Stamina { get; }
		public CharacteristicProxy Perception { get; }
		public CharacteristicProxy Intelligence { get; }
		public CharacteristicProxy Luck { get; }

		public PrimaryCharacteristicsProxy(PrimaryCharacteristics primary)
		{
			Strength = new CharacteristicProxy(primary.strength);
			Dexterity = new CharacteristicProxy(primary.dexterity);
			Stamina = new CharacteristicProxy(primary.stamina);
			Perception = new CharacteristicProxy(primary.perception);
			Intelligence = new CharacteristicProxy(primary.intelligence);
			Luck = new CharacteristicProxy(primary.luck);
		}
	}
}