using Infrastructure.State.Entities.Player.Characteristics.Data;
using UnityEngine;

namespace Infrastructure.State.Entities.Player.Characteristics.Proxy
{
	public sealed class CharacteristicsDataProxy
	{
		public PrimaryCharacteristicsProxy Primary { get; }
		public SecondaryCharacteristicsProxy Secondary{ get; }
		

		public CharacteristicsDataProxy(CharacteristicsData characteristics)
		{
			Primary = new PrimaryCharacteristicsProxy(characteristics.primary);
			Secondary = new SecondaryCharacteristicsProxy(characteristics.secondary);
		}
	}
}