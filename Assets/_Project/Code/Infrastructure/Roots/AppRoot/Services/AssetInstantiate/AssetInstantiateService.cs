using Infrastructure.DI;
using Infrastructure.Roots.AppRoot.Services.ResourceLoader;
using UnityEngine;
using UserCamera;
using Utils.Coroutiner;

namespace Infrastructure.Roots.AppRoot.Services.AssetInstantiate
{
	public class AssetInstantiateService : IAssetInstantiateService
	{
		private readonly DIContainer _diContainer;

		public AssetInstantiateService(DIContainer diContainer)
		{
			_diContainer = diContainer;
		}

		public CoroutineRunner GetCoroutineRunner()
		{
			var coroutineRunner = new GameObject("[COROUTINE RUNNER]")
				.AddComponent<CoroutineRunner>();
			Object.DontDestroyOnLoad(coroutineRunner.gameObject);

			return coroutineRunner;
		}

		public CameraController GetCameraController() => GetInstance(_diContainer.Resolve<IResourceLoaderService>().GetPrefabCameraController());

		public T GetInstance<T>(T prefab, Transform root = null, Vector3 position = default, Quaternion rotation = default, bool bisDontDestroyOnLoad = false)
			where T : MonoBehaviour
		{
			var instance = Object.Instantiate(prefab, position, Quaternion.identity, root);
			if (bisDontDestroyOnLoad) Object.DontDestroyOnLoad(instance.gameObject);
			return instance;
		}
	}
}