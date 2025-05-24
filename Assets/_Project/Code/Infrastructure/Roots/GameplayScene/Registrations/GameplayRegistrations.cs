using Constants;
using Infrastructure.DI;
using Infrastructure.Roots.GameplayScene.EnterExitParams;
using Infrastructure.State.GameResources.Services;
using R3;

namespace Infrastructure.Roots.GameplayScene.Registrations
{
	public static class GameplayRegistrations
	{
		public static void Register(DIContainer diContainer, GameplayEnterParams enterParams)
		{
			diContainer.RegisterInstance(SignalTags.EXIT_TO_MAIN_MENU_SCENE_REQUEST, new Subject<Unit>());
			diContainer.RegisterInstance(SignalTags.EXIT_TO_GARAGE_SCENE_REQUEST, new Subject<Unit>());

			diContainer.RegisterFactory(c => new ResourcesService(c)).AsSingle();
		}
	}
}