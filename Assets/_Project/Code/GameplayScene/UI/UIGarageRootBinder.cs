using System;
using Extensions;
using MainMenu.UI;
using UnityEngine;

namespace GameplayScene.UI
{
	public class UIGameplayRootBinder : MonoBehaviour
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