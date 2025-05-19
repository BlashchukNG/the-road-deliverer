using System;
using Infrastructure.State.Entities.Player.Characteristics.Data;
using Infrastructure.State.Entities.Player.Characteristics.Proxy;
using R3;

namespace Logic.Characters.Base
{
	[Serializable]
	public sealed class CharacterCharacteristics
	{
		private readonly CharacteristicsDataProxy _characteristics;

		//Calculated
		public float MoveSpeed { get; private set; }
		public float MaxWeight { get; private set; }

		public float Weight { get; private set; }

		public CharacterCharacteristics(CharacteristicsDataProxy data)
		{
			_characteristics = data;
			data.Primary.Strength.Level.Subscribe(_ => Calculate(data.Primary.Strength));
		}

		public void Calculate(CharacteristicProxy data)
		{
			switch (data.Type)
			{
				case CharacteristicType.Strength:
					CalculateStrengthDependencies();
					break;
				case CharacteristicType.Dexterity:
					CalculateDexterityDependencies();
					break;
				case CharacteristicType.Stamina:
					break;
				case CharacteristicType.Perception:
					break;
				case CharacteristicType.Intelligence:
					break;
				case CharacteristicType.Luck:
					break;
				case CharacteristicType.Athleticism:
					break;
				case CharacteristicType.Driving:
					break;
				case CharacteristicType.None:
					throw new Exception("Characteristic: empty type");
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void CalculateStrengthDependencies()
		{
		}

		private void CalculateDexterityDependencies()
		{
			CalculateMoveSpeed();
		}

		private void CalculateMoveSpeed()
		{
			// Базовые параметры
			var dexterityCoefficient = 0.1f;    // Коэффициент ловкости (k_A)
			var athleticsCoefficient = 0.08f; // Коэффициент атлетизма (k_Ath)
			var baseSpeed = 6.0f;             // Базовая скорость

			// Характеристики персонажа
			var dexterity = _characteristics.Primary.Dexterity.Level.Value;     // Ловкость (A)
			var athletics = _characteristics.Secondary.Athleticism.Level.Value; // Атлетизм (Ath)

			// Вес экипировки
			var currentWeight = Weight; // Текущий вес (Weight)
			var maxWeight = MaxWeight;     // Макс. грузоподъемность (MaxWeight)

			// Расчет скорости по формуле
			float numerator = baseSpeed +
			                  (dexterityCoefficient * dexterity) +
			                  (athleticsCoefficient * athletics);

			float denominator = 1.0f + (currentWeight / maxWeight);

			MoveSpeed = numerator / denominator;
		}
	}
}