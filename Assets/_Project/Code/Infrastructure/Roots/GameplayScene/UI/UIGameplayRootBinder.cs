using Extensions;
using R3;
using UnityEngine;

namespace Infrastructure.Roots.GameplayScene.UI
{
	public class UIGameplayRootBinder : MonoBehaviour
	{
		[SerializeField] private GameplayButton _buttonToGarage;
		[SerializeField] private GameplayButton _buttonToMainMenu;

		private Subject<Unit> _exitToMainMenuSubject;
		private Subject<Unit> _exitToGarageSubject;

		private void Awake()
		{
			_buttonToMainMenu.Button.AddOneListener(HandleButtonToMainMenuClicked);
			_buttonToGarage.Button.AddOneListener(HandleButtonToGarageClicked);
		}

		public void Bind(Subject<Unit> exitToMainMenuSubject, Subject<Unit> exitToGameplaySubject)
		{
			_exitToMainMenuSubject = exitToMainMenuSubject;
			_exitToGarageSubject = exitToGameplaySubject;
		}

		public void HandleButtonToMainMenuClicked()
		{
			_exitToMainMenuSubject?.OnNext(Unit.Default);
		}

		public void HandleButtonToGarageClicked()
		{
			_exitToGarageSubject?.OnNext(Unit.Default);
		}
	}
}