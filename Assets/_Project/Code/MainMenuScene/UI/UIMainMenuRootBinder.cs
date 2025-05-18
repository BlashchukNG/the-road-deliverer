using System;
using Extensions;
using UnityEngine;

namespace MainMenu.UI
{
	public class UIMainMenuRootBinder : MonoBehaviour
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