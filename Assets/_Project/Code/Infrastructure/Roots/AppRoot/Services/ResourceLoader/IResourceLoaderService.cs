using UserCamera;

namespace Infrastructure.Roots.AppRoot.Services.ResourceLoader
{
	public interface IResourceLoaderService
	{
		UIRootView GetPrefabUIRootView();
		CameraController GetPrefabCameraController();
	}
}