using Code.Utils.Coroutiner;
using UnityEngine;

namespace Infrastructure.Services.AssetInstantiate
{
	public interface IAssetInstantiateService
	{
		CoroutineRunner GetCoroutineRunner();
		T GetInstance<T>(T prefab, Transform root = null, Vector3 position = new(), Quaternion rotation = new(), bool bisDontDestroyOnLoad = false) where T : Component;
	}
}