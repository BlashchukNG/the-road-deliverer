using Infrastructure.State.Entities.Player.Characteristics.Data;

namespace Infrastructure.State.Entities.Player.Characteristics.Proxy
{
	public sealed class SecondaryCharacteristicsProxy
	{
		public CharacteristicProxy Athleticism { get; }
		public CharacteristicProxy Driving { get; }

		public SecondaryCharacteristicsProxy(SecondaryCharacteristics secondary)
		{
			Athleticism = new CharacteristicProxy(secondary.athleticism);
			Driving = new CharacteristicProxy(secondary.driving);
		}
	}
}