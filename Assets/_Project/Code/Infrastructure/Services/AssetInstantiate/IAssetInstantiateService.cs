using UnityEngine;
using UserCamera;
using Utils.Coroutiner;

namespace Infrastructure.Services.AssetInstantiate
{
	public interface IAssetInstantiateService
	{
		CoroutineRunner GetCoroutineRunner();
		T GetInstance<T>(T prefab, Transform root = null, Vector3 position = default, Quaternion rotation = default, bool bisDontDestroyOnLoad = false) where T : MonoBehaviour;
		CameraController GetCameraController();
	}
}