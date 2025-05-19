using Constants;
using Infrastructure.Roots.AppRoot.Abstract;

namespace Infrastructure.Roots.MainMenuScene.EnterExitParams
{
	public sealed class MainMenuEnterParams : BaseSceneEnterParams
	{
		public string DebugData { get; }

		public MainMenuEnterParams(string debugData) : base(Scenes.MAIN_MENU)
		{
			DebugData = debugData;
		}
	}
}