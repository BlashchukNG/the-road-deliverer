using Infrastructure.DI;
using Infrastructure.Roots.GarageScene.EnterExitParams;
using Infrastructure.State.CMD;
using Infrastructure.State.CMD.Handlers.GameResources;
using Infrastructure.State.GameResources.Services;

namespace Infrastructure.Roots.GarageScene.Registrations
{
	public static class GarageRegistrations
	{
		public static void Register(DIContainer diContainer, GarageEnterParams enterParams)
		{
			var cmd = new CommandProcessor(diContainer);
			cmd.RegisterHandler(new CMDResourcesAddHandler(diContainer));
			cmd.RegisterHandler(new CMDResourcesSpendHandler(diContainer));
			diContainer.RegisterInstance(cmd);

			diContainer.RegisterFactory(c => new ResourcesService(c)).AsSingle();
		}
	}
}