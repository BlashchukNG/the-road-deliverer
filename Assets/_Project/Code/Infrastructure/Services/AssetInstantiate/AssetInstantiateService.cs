using Code.Utils.Coroutiner;
using UnityEngine;

namespace Infrastructure.Services.AssetInstantiate
{
	public class AssetInstantiateService : IAssetInstantiateService
	{
		public AssetInstantiateService()
		{
		}

		public CoroutineRunner GetCoroutineRunner()
		{
			var coroutineRunner = new GameObject("[COROUTINE RUNNER]")
				.AddComponent<CoroutineRunner>();
			Object.DontDestroyOnLoad(coroutineRunner.gameObject);

			return coroutineRunner;
		}

		public T GetInstance<T>(T prefab, Transform root = null, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), bool bisDontDestroyOnLoad = false)
			where T : Component
		{
			var instance = Object.Instantiate(prefab, position, Quaternion.identity, root);
			if (bisDontDestroyOnLoad) Object.DontDestroyOnLoad(instance.gameObject);
			return instance;
		}
	}
}