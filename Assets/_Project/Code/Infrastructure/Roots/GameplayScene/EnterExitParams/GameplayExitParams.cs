using Infrastructure.Roots.AppRoot.Abstract;

namespace Infrastructure.Roots.GameplayScene.EnterExitParams
{
	public sealed class GameplayExitParams
	{
		public BaseSceneEnterParams TargetSceneEnterParams { get; }

		public GameplayExitParams(BaseSceneEnterParams enterParams)
		{
			TargetSceneEnterParams = enterParams;
		}
	}
}