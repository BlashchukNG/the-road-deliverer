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
		private Subject<Unit> _exitToGameplaySubject;

		private void Awake()
		{
			_buttonToMainMenu.Button.AddOneListener(HandleButtonToMainMenuClicked);
			_buttonToGameplay.Button.AddOneListener(HandleButtonToGameplayClicked);
		}

		public void Bind(Subject<Unit> exitToMainMenuSubject, Subject<Unit> exitToGameplaySubject)
		{
			_exitToMainMenuSubject = exitToMainMenuSubject;
			_exitToGameplaySubject = exitToGameplaySubject;
		}

		public void HandleButtonToMainMenuClicked()
		{
			_exitToMainMenuSubject?.OnNext(Unit.Default);
		}
		
		public void HandleButtonToGameplayClicked()
		{
			_exitToGameplaySubject?.OnNext(Unit.Default);
		}
	}
}