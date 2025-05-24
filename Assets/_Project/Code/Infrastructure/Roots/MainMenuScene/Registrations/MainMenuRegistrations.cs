using Constants;
using Infrastructure.DI;
using Infrastructure.Roots.MainMenuScene.EnterExitParams;
using R3;

namespace Infrastructure.Roots.MainMenuScene.Registrations
{
	public static class MainMenuRegistrations
	{
		public static void Register(DIContainer diContainer, MainMenuEnterParams enterParams)
		{
			diContainer.RegisterInstance(SignalTags.EXIT_SCENE_REQUEST, new Subject<Unit>());
		}
	}
}