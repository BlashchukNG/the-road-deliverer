using Extensions;
using R3;
using UnityEngine;

namespace Infrastructure.Roots.MainMenuScene.Views
{
	public sealed class UIMainMenuRootBinder : MonoBehaviour
	{
		[SerializeField] private MainMenuButton _buttonPlay;

		private Subject<Unit> _exitToMainMenuSubject;

		private void Awake()
		{
			_buttonPlay.Button.AddOneListener(HandleButtonPlayClicked);
		}

		public void Bind(Subject<Unit> exitToMainMenuSubject)
		{
			_exitToMainMenuSubject = exitToMainMenuSubject;
		}

		public void HandleButtonPlayClicked()
		{
			_exitToMainMenuSubject?.OnNext(Unit.Default);
		}
	}
}