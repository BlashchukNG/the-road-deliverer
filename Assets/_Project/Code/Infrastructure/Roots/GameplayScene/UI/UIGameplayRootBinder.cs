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

		private void Awake()
		{
			_buttonToMainMenu.Button.AddOneListener(HandleButtonToMainMenuClicked);
			_buttonToGarage.Button.AddOneListener(HandleButtonToGarageClicked);
		}

		public void Bind(Subject<Unit> exitToMainMenuSubject)
		{
			_exitToMainMenuSubject = exitToMainMenuSubject;
		}

		public void HandleButtonToMainMenuClicked()
		{
			_exitToMainMenuSubject?.OnNext(Unit.Default);
		}

		public void HandleButtonToGarageClicked()
		{
			//_exitToMainMenuSubject?.OnNext(Unit.Default);
		}
	}
}