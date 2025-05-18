namespace Infrastructure.Roots.GameplayScene.EnterExitParams
{
	public class GameplayExitParams
	{
		public string DebugData { get; }

		public GameplayExitParams(string debugData)
		{
			DebugData = debugData;
		}
	}
}