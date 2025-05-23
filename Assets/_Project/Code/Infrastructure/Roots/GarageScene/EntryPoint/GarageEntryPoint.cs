using Infrastructure.DI;
using Infrastructure.Roots.GameplayScene.EnterExitParams;
using Infrastructure.Roots.GarageScene.EnterExitParams;
using Infrastructure.Roots.GarageScene.Registrations;
using Infrastructure.Roots.GarageScene.View;
using Infrastructure.Roots.MainMenuScene.EnterExitParams;
using R3;
using UnityEngine;

namespace Infrastructure.Roots.GarageScene.EntryPoint
{
	public sealed class GarageEntryPoint : MonoBehaviour
	{
		private DIContainer _diContainer;
		private DIContainer _viewModelDIContainer;

		public Observable<GarageExitParams> Run(DIContainer diContainer, GarageEnterParams enterParams)
		{
			_diContainer = diContainer;
			GarageRegistrations.Register(_diContainer, enterParams);
			_viewModelDIContainer = new DIContainer(_diContainer);
			GarageViewModelRegistrations.Register(_viewModelDIContainer);

			var exitToMainMenuSignalSubject = new Subject<Unit>();
			var exitToGameplaySignalSubject = new Subject<Unit>();

			_viewModelDIContainer.Resolve<UIGarageRootBinder>().Bind(exitToMainMenuSignalSubject, exitToGameplaySignalSubject);

			var mainMenuEnterParams = new MainMenuEnterParams("from garage");
			var gameplayEnterParams = new GameplayEnterParams("from garage");

			var exitToMainMenuParams = new GarageExitParams(mainMenuEnterParams);
			var exitToGameplayParams = new GarageExitParams(gameplayEnterParams);

			var exitSignal = exitToMainMenuSignalSubject
			                 .Select(_ => exitToMainMenuParams)
			                 .Merge(exitToGameplaySignalSubject.Select(_ => exitToGameplayParams));


			return exitSignal;
		}
	}
}