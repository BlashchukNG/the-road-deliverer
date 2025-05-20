using Infrastructure.State;
using Infrastructure.State.Entities.Player.Characteristics;
using Logic.UserCamera;

namespace Infrastructure.Roots.AppRoot.Services.ResourceLoader
{
	public interface IResourceLoaderService
	{
		UIRootView GetPrefabUIRootView();
		CameraController GetPrefabCameraController();
		SaveFileConfig GetBaseSaveFile();
	}
}