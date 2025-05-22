using Infrastructure.State;
using Logic.Characters.Base.Locomotions;
using Logic.UserCamera;

namespace Infrastructure.Roots.AppRoot.Services.ResourceLoader
{
	public interface IResourceLoaderService
	{
		UIRootView GetPrefabUIRootView();
		CameraController GetPrefabCameraController();
		SaveFileConfig GetBaseSaveFile();
		LocomotionSettings GetLocomotionSettings();
	}
}