using UnityEngine;

namespace Logic.InteractableEntities.Doors
{
	public sealed class ControlPanel : MonoBehaviour
	{
		[SerializeField] private Door _door;
		[SerializeField] private GameObject[] toEnable;
		private bool _canInteract;
		private bool _isDoorOpen;


		private void Awake()
		{
			foreach (var go in toEnable)
				go.SetActive(false);
		}

		private void OnTriggerEnter(Collider other)
		{
			_canInteract = true;
			foreach (var go in toEnable)
				go.SetActive(true);
		}

		private void OnTriggerExit(Collider other)
		{
			_canInteract = false;
			foreach (var go in toEnable)
				go.SetActive(false);
		}

		private void MouseInteract()
		{
			if(_canInteract) Interact();
		}

		public void Interact()
		{
			if(_door.isBusy) return;
			
			_isDoorOpen = !_isDoorOpen;
			_door.SetAction(_isDoorOpen);
		}
	}
}