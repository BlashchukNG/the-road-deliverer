using Infrastructure.AppRoot.Abstract;

namespace Infrastructure.Roots.GameplayScene.EnterExitParams
{
	public class GameplayExitParams
	{
		public BaseSceneEnterParams TargetSceneEnterParams { get; }

		public GameplayExitParams(BaseSceneEnterParams enterParams)
		{
			TargetSceneEnterParams = enterParams;
		}
	}
}