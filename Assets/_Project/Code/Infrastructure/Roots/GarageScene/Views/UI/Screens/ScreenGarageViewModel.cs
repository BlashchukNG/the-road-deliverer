using Infrastructure.Roots.GarageScene.Services.UI;
using Infrastructure.Roots.MainMenuScene.Services.UI;
using R3;
using VVM.UI;

namespace Infrastructure.Roots.GarageScene.Views.UI.Screens
{
	public sealed class ScreenGarageViewModel : WindowViewModel
	{
		private readonly GarageUIService _uiService;
		private readonly Subject<Unit> _exitSceneToMainMenuRequest;
		private readonly Subject<Unit> _exitSceneToGameplayRequest;

		public ScreenGarageViewModel(GarageUIService uiService, Subject<Unit> exitSceneToMainMenuRequest, Subject<Unit> exitSceneToGameplayRequest)
		{
			_uiService = uiService;
			_exitSceneToMainMenuRequest = exitSceneToMainMenuRequest;
			_exitSceneToGameplayRequest = exitSceneToGameplayRequest;
		}

		public override string Id => "screen garage";

		public void RequestGoToMainMenuScene() => _exitSceneToMainMenuRequest.OnNext(Unit.Default);
		public void RequestGoToGameplayScene() => _exitSceneToGameplayRequest.OnNext(Unit.Default);
	}
}