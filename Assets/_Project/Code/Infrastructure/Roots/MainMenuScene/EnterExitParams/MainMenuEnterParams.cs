using Constants;
using Infrastructure.AppRoot.Abstract;

namespace Infrastructure.Roots.MainMenuScene.EnterExitParams
{
	public class MainMenuEnterParams : BaseSceneEnterParams
	{
		public string DebugData { get; }

		public MainMenuEnterParams(string debugData) : base(Scenes.MAIN_MENU)
		{
			DebugData = debugData;
		}
	}
}