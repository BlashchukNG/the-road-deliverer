using System;
using Logic.UserCamera;
using UnityEngine;

namespace Logic.Characters.Player
{
	public sealed class PlayerBinder : MonoBehaviour
	{
		public Transform Transform => _characterController.transform;
		public CharacterController CharacterController => _characterController;
		public Animator Animator => _animator;
		public Transform FrontRayPos => _frontRayPos;
		public Transform RearRayPos => _rearRayPos;
		
		public event Action onDestroy;
		

		[SerializeField] private CharacterController _characterController;
		[SerializeField] private Animator _animator;
		[SerializeField] private Transform _frontRayPos;
		[SerializeField] private Transform _rearRayPos;

		private void OnDestroy() => onDestroy?.Invoke();

		public void Bind(PlayerViewModel viewModel, CameraController camera)
		{
			viewModel.AttachReferences(this, camera);
		}
	}
}