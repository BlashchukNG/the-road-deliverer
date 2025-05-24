using Constants;
using Infrastructure.DI;
using Infrastructure.Roots.GarageScene.EnterExitParams;
using Infrastructure.State.CMD;
using Infrastructure.State.CMD.Handlers.GameResources;
using Infrastructure.State.GameResources.Services;
using R3;

namespace Infrastructure.Roots.GarageScene.Registrations
{
	public static class GarageRegistrations
	{
		public static void Register(DIContainer diContainer, GarageEnterParams enterParams)
		{
			diContainer.RegisterInstance(SignalTags.EXIT_TO_MAIN_MENU_SCENE_REQUEST, new Subject<Unit>());
			diContainer.RegisterInstance(SignalTags.EXIT_TO_GAMEPLAY_SCENE_REQUEST, new Subject<Unit>());
			
			var cmd = new CommandProcessor(diContainer);
			cmd.RegisterHandler(new CMDResourcesAddHandler(diContainer));
			cmd.RegisterHandler(new CMDResourcesSpendHandler(diContainer));
			diContainer.RegisterInstance(cmd);

			diContainer.RegisterFactory(c => new ResourcesService(c)).AsSingle();
		}
	}
}