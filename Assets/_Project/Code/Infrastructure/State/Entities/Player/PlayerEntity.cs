using System;
using Infrastructure.State.Entities.Abstract;
using Infrastructure.State.Entities.Player.Characteristics.Data;

namespace Infrastructure.State.Entities.Player
{
	[Serializable]
	public sealed class PlayerEntity : Entity, ICloneable
	{
		public CharacteristicsData characteristics;

		public object Clone() =>
			new PlayerEntity
			{
				characteristics = this.characteristics.Clone() as CharacteristicsData,
			};
	}
}