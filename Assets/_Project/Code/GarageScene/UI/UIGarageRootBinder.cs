using System;
using Extensions;
using MainMenu.UI;
using UnityEngine;

namespace GarageScene.UI
{
	public class UIGarageRootBinder : MonoBehaviour
	{
		public event Action onPlayButtonClicked;

		[SerializeField] private MainMenuButton _buttonPlay;

		private void Awake()
		{
			_buttonPlay.Button.AddOneListener(HandlePlayButtonClicked);
		}

		public void HandlePlayButtonClicked()
		{
			onPlayButtonClicked?.Invoke();
		}
	}
}