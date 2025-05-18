using Infrastructure.AppRoot;
using UserCamera;

namespace Infrastructure.Services.ResourceLoader
{
	public interface IResourceLoaderService
	{
		UIRootView GetPrefabUIRootView();
		CameraController GetPrefabCameraController();
	}
}