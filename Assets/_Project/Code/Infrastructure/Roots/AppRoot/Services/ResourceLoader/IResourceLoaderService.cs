using Infrastructure.State;
using Logic.Characters.Base.Locomotions;
using Logic.UserCamera;
using UnityEngine;

namespace Infrastructure.Roots.AppRoot.Services.ResourceLoader
{
	public interface IResourceLoaderService
	{
		UIRootView GetPrefabUIRootView();
		CameraController GetPrefabCameraController();
		SaveFileConfig GetBaseSaveFile();
		LocomotionSettings GetPlayerLocomotionSettings();
		T GetPrefab<T>(string name)
			where T : MonoBehaviour;
	}
}