using Infrastructure.Roots.GameplayScene.Services.UI;
using R3;
using VVM.UI;

namespace Infrastructure.Roots.GameplayScene.View.UI.Screens
{
	public sealed class ScreenGameplayViewModel : WindowViewModel
	{
		private readonly GameplayUIService _uiService;
		private readonly Subject<Unit> _exitSceneToMainMenuRequest;
		private readonly Subject<Unit> _exitSceneToGarageRequest;

		public ScreenGameplayViewModel(GameplayUIService uiService, Subject<Unit> exitSceneToMainMenuRequest, Subject<Unit> exitSceneToGarageRequest)
		{
			_uiService = uiService;
			_exitSceneToMainMenuRequest = exitSceneToMainMenuRequest;
			_exitSceneToGarageRequest = exitSceneToGarageRequest;
		}

		public override string Id => "screen gameplay";

		public void RequestGoToMainMenuScene() => _exitSceneToMainMenuRequest.OnNext(Unit.Default);
		public void RequestGoToGarageScene() => _exitSceneToGarageRequest.OnNext(Unit.Default);
	}
}