using Infrastructure.Roots.MainMenuScene.Services.UI;
using R3;
using VVM.UI;

namespace Infrastructure.Roots.MainMenuScene.Views.UI.Screens
{
	public sealed class ScreenMainMenuViewModel : WindowViewModel
	{
		private readonly MainMenuUIService _uiService;
		private readonly Subject<Unit> _exitSceneRequest;

		public ScreenMainMenuViewModel(MainMenuUIService uiService, Subject<Unit> exitSceneRequest)
		{
			_uiService = uiService;
			_exitSceneRequest = exitSceneRequest;
		}

		public override string Id => "screen main menu";

		public void RequestOpenSettingsPopup() => _uiService.OpenPopupSettings();
		public void RequestGoToGarageScene() => _exitSceneRequest.OnNext(Unit.Default);
	}
}