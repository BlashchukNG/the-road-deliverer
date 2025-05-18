using Extensions;
using Infrastructure.Roots.MainMenuScene.UI;
using R3;
using UnityEngine;

namespace Infrastructure.Roots.GarageScene.UI
{
	public class UIGarageRootBinder : MonoBehaviour
	{
		[SerializeField] private GarageButton _buttonToMainMenu;
		[SerializeField] private GarageButton _buttonToGameplay;

		private Subject<Unit> _exitToMainMenuSubject;

		private void Awake()
		{
			_buttonToMainMenu.Button.AddOneListener(HandleButtonToMainMenuClicked);
			_buttonToGameplay.Button.AddOneListener(HandleButtonToGameplayClicked);
		}

		public void Bind(Subject<Unit> exitToMainMenuSubject)
		{
			_exitToMainMenuSubject = exitToMainMenuSubject;
		}

		public void HandleButtonToMainMenuClicked()
		{
			_exitToMainMenuSubject?.OnNext(Unit.Default);
		}
		
		public void HandleButtonToGameplayClicked()
		{
			//_exitToMainMenuSubject?.OnNext(Unit.Default);
		}
	}
}