using System;
using UnityEngine;

namespace Infrastructure.State.Entities.Player.Characteristics.Data
{
	[Serializable]
	public sealed class CharacteristicsData : MonoBehaviour
	{
		public PrimaryCharacteristics primary;
		public SecondaryCharacteristics secondary;
	}
}