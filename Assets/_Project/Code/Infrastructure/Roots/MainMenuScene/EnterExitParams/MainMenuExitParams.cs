using Infrastructure.Roots.GarageScene.EnterExitParams;

namespace Infrastructure.Roots.MainMenuScene.EnterExitParams
{
	public class MainMenuExitParams
	{
		public GarageEnterParams GarageExitParams { get; }

		public MainMenuExitParams(GarageEnterParams garageExitParams)
		{
			GarageExitParams = garageExitParams;
		}
	}
}