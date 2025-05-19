using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using UserCamera;

namespace Infrastructure.Roots.AppRoot.Services.ResourceLoader
{
	public class ResourceLoaderService : IResourceLoaderService
	{
		private readonly Dictionary<string, string> _resources = new();

		public ResourceLoaderService()
		{
			var config = Resources.Load<GeneralResourcesConfig>(ResourcesConstants.CONFIG_GENERAL_RESOURCES);

			foreach (var resource in config.resources)
			{
				if (!_resources.TryAdd(resource.name, resource.path))
					throw new Exception($"ResourceLoader: {resource.name} already exists");
			}
		}

		public UIRootView GetPrefabUIRootView() => Load<UIRootView>(ResourcesConstants.UI_ROOT_VIEW);
		public CameraController GetPrefabCameraController() => Load<CameraController>(ResourcesConstants.CAMERA_CONTROLLER);
		private T Load<T>(string name) where T : MonoBehaviour => Resources.Load<T>(_resources[name]);
	}
}