using Infrastructure.Roots.AppRoot.Services.Updater;
using UnityEngine;
using UserCamera;
using Utils.Coroutiner;

namespace Infrastructure.Roots.AppRoot.Services.AssetInstantiate
{
	public interface IAssetInstantiateService
	{
		CoroutineRunner GetCoroutineRunner();
		IUpdateService GetUpdater();
		CameraController GetCameraController();
		T GetInstance<T>(T prefab, Transform root = null, Vector3 position = default, Quaternion rotation = default, bool bisDontDestroyOnLoad = false) where T : MonoBehaviour;
	}
}