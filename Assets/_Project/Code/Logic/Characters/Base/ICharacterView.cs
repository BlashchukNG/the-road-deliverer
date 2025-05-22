using System;
using UnityEngine;

namespace Logic.Characters.Base
{
	public interface ICharacterView
	{
		public Transform Transform { get; }
		public CharacterController CharacterController { get; }
		public Animator Animator { get; }
		Transform FrontRayPos { get; }
		Transform RearRayPos { get; }

		public event Action onDestroy;
	}
}