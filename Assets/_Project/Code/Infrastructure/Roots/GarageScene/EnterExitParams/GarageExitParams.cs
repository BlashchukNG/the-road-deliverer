using Infrastructure.Roots.MainMenuScene.EnterExitParams;

namespace Infrastructure.Roots.GarageScene.EnterExitParams
{
	public class GarageExitParams
	{
		public MainMenuEnterParams MainMenuEnterParams { get; }

		public GarageExitParams(MainMenuEnterParams mainMenuEnterParams)
		{
			MainMenuEnterParams = mainMenuEnterParams;
		}
	}
}