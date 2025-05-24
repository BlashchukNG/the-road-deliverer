using Infrastructure.DI;
using Infrastructure.Roots.GarageScene.Services.UI;
using Infrastructure.State.GameResources.Services;
using Infrastructure.State.GameResources.View;
using ObservableCollections;
using R3;
using VVM.UI;

namespace Infrastructure.Roots.GarageScene.Views.UI.Screens
{
	public sealed class ScreenGarageViewModel : WindowViewModel
	{
		private readonly DIContainer _diContainer;
		private readonly GarageUIService _uiService;
		private readonly Subject<Unit> _exitSceneToMainMenuRequest;
		private readonly Subject<Unit> _exitSceneToGameplayRequest;
		private readonly ObservableList<ResourceViewModel> _resources;

		public IObservableCollection<ResourceViewModel> Resources => _resources;

		public ScreenGarageViewModel(DIContainer diContainer, Subject<Unit> exitSceneToMainMenuRequest, Subject<Unit> exitSceneToGameplayRequest)
		{
			_diContainer = diContainer;
			_uiService = diContainer.Resolve<GarageUIService>();
			_resources = diContainer.Resolve<ResourcesService>().Resources;
			
			_exitSceneToMainMenuRequest = exitSceneToMainMenuRequest;
			_exitSceneToGameplayRequest = exitSceneToGameplayRequest;
		}

		public override string Id => "screen garage";

		public void RequestGoToMainMenuScene() => _exitSceneToMainMenuRequest.OnNext(Unit.Default);
		public void RequestGoToGameplayScene() => _exitSceneToGameplayRequest.OnNext(Unit.Default);
	}
}