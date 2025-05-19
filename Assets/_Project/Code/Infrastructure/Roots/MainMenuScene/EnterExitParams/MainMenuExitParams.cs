using Infrastructure.Roots.GarageScene.EnterExitParams;

namespace Infrastructure.Roots.MainMenuScene.EnterExitParams
{
	public sealed class MainMenuExitParams
	{
		public GarageEnterParams GarageExitParams { get; }

		public MainMenuExitParams(GarageEnterParams garageExitParams)
		{
			GarageExitParams = garageExitParams;
		}
	}
}