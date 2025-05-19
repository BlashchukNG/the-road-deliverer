using System;
using Infrastructure.State.Entities.Abstract;
using Infrastructure.State.Entities.Player.Characteristics;
using Infrastructure.State.Entities.Player.Characteristics.Data;

namespace Infrastructure.State.Entities.Player
{
	[Serializable]
	public sealed class PlayerEntity : Entity
	{
		public CharacteristicsData characteristics;
	}
}