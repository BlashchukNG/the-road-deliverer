using System;

namespace Infrastructure.State.Entities.Player.Characteristics.Data
{
	[Serializable]
	public enum CharacteristicType
	{
		None = 0,
		//primary
		Strength = 1,
		Dexterity = 2,
		Stamina = 3,
		Perception = 4,
		Intelligence = 5,
		Luck = 6,
		//secondary
		Athleticism = 101,
		Driving = 102,
	}
}