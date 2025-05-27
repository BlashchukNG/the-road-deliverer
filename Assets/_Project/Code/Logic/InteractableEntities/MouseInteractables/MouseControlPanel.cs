using Logic.InteractableEntities.Doors;
using Logic.InteractableEntities.MouseInteractables.Abstract;
using UnityEngine;

namespace Logic.InteractableEntities.MouseInteractables
{
	public sealed class MouseControlPanel : BaseMouseInteractableObject
	{
		[SerializeField] private ControlPanel _panel;
		
		public override void Interact() => _panel.Interact();
	}
}