using Infrastructure.Roots.GameplayScene.EnterExitParams;
using Infrastructure.Roots.GarageScene.EnterExitParams;
using Infrastructure.Roots.MainMenuScene.EnterExitParams;

namespace Infrastructure.Services.SceneLoader
{
	public interface ISceneLoaderService
	{
		void LoadMainMenu(MainMenuEnterParams enterParams = null);
		void LoadGarage(GarageEnterParams enterParams = null);
		void LoadGameplay(GameplayEnterParams enterParams = null);
	}
}