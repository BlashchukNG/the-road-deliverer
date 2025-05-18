using Constants;
using Infrastructure.AppRoot.Abstract;

namespace Infrastructure.Roots.GameplayScene.EnterExitParams
{
	public class GameplayEnterParams : BaseSceneEnterParams
	{
		public string DebugData { get; }
		
		public GameplayEnterParams(string debugData) : base(Scenes.GAMEPLAY)
		{
			DebugData = debugData;
		}
	}
}