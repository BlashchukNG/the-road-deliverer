using Constants;
using Infrastructure.Roots.AppRoot.Abstract;

namespace Infrastructure.Roots.GarageScene.EnterExitParams
{
	public class GarageEnterParams : BaseSceneEnterParams
	{
		public string DebugData { get; }
		
		public GarageEnterParams(string debugData) : base(Scenes.GARAGE)
		{
			DebugData = debugData;
		}
	}
}