using System;
using Constants;
using Extensions;
using Infrastructure.State.Entities.Player.Characteristics.Data;
using R3;
using UnityEngine;

namespace Infrastructure.State.Entities.Player.Characteristics.Proxy
{
	public sealed class CharacteristicProxy
	{
		public string TitleLocKey { get; }
		public string DescriptionLocKey { get; }

		public CharacteristicType Type { get; }
		public ReactiveProperty<int> Level { get; }
		public ReactiveProperty<float> Progress { get; }
		public ReactiveProperty<float> Modificator { get; }

		private float _modToLinearProgression;
		private float _modToQuadraticProgression;

		public CharacteristicProxy(Characteristic characteristic)
		{
			TitleLocKey = characteristic.titleLocKey;
			DescriptionLocKey = characteristic.descriptionLocKey;

			Type = characteristic.type;

			Level = new ReactiveProperty<int>(characteristic.level);
			Level.Skip(1).Subscribe(value => characteristic.level = value);

			Progress = new ReactiveProperty<float>(characteristic.progress);
			Progress.Skip(1).Subscribe(value => characteristic.progress = value);

			Modificator = new ReactiveProperty<float>(characteristic.modificator);
			Modificator.Skip(1).Subscribe(value => characteristic.modificator = value);
		}

		public void AddProgress()
		{
			Progress.Value += 1f * Modificator.Value;
			if (Progress.Value >= CalculateLevelUpProgress())
			{
				_modToLinearProgression = 0;
				_modToQuadraticProgression = 0;

				Progress.Value = 0f;
				Level.Value++;
			}
		}

		private int CalculateLevelUpProgress()
		{
			var result = 0;

			switch (Type)
			{
				case CharacteristicType.Strength:
				case CharacteristicType.Dexterity:
				case CharacteristicType.Stamina:
				case CharacteristicType.Perception:
				case CharacteristicType.Intelligence:
				case CharacteristicType.Luck:
					result = QuadraticProgression().ToFloat();
					break;
				case CharacteristicType.Athleticism:
				case CharacteristicType.Driving:
					result = LinearProgression().ToFloat();
					break;
				case CharacteristicType.None:
					throw new Exception("Characteristic: empty type");
				default:
					throw new ArgumentOutOfRangeException();
			}

			return result;
		}

		private float LinearProgression() =>
			_modToLinearProgression == 0
				? _modToLinearProgression = CharacteristicConstants.TO_LINEAR_PROGRESSION * Level.Value
				: _modToLinearProgression;

		private float QuadraticProgression() =>
			_modToQuadraticProgression == 0
				? _modToQuadraticProgression = CharacteristicConstants.TO_QUADRATIC_PROGRESSION * Mathf.Pow(Level.Value, 2)
				: _modToQuadraticProgression;
	}
}