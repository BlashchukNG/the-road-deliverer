using System;
using Logic.Characters.Base;
using UnityEngine;

namespace Logic.Characters.Player
{
	public sealed class PlayerView : MonoBehaviour, ICharacterView
	{
		public Transform Transform => _characterController.transform;
		public CharacterController CharacterController => _characterController;
		public Animator Animator => _animator;
		public event Action onDestroy;

		[SerializeField] public CharacterController _characterController;
		[SerializeField] public Animator _animator;

		private void OnDestroy() => onDestroy?.Invoke();
	}
}