using UnityEngine;

namespace Logic.Characters.Player
{
	public sealed class PlayerView : MonoBehaviour
	{
		public Transform Transform => transform;
		public CharacterController CharacterController => _characterController;
		public Animator Animator => _animator;

		[SerializeField] public CharacterController _characterController;
		[SerializeField] public Animator _animator;
	}
}