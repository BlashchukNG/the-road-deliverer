using Infrastructure.State;
using Logic.Characters.Player.Locomotions;
using Logic.UserCamera;
using UnityEngine;

namespace Infrastructure.Roots.AppRoot.Services.ResourceLoader
{
	public interface IResourceLoaderService
	{
		UIRootView GetPrefabUIRootView();
		CameraController GetPrefabCameraController();
		SaveFileConfig GetBaseSaveFile();
		Settings GetPlayerLocomotionSettings();
		T GetPrefab<T>(string name)
			where T : MonoBehaviour;
	}
}