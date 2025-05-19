using Infrastructure.Roots.AppRoot.Abstract;

namespace Infrastructure.Roots.GarageScene.EnterExitParams
{
	public class GarageExitParams
	{
		public BaseSceneEnterParams TargetSceneEnterParams { get; }

		public GarageExitParams(BaseSceneEnterParams enterParams)
		{
			TargetSceneEnterParams = enterParams;
		}
	}
}