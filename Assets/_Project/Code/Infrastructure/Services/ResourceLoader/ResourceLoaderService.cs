using System;
using System.Collections.Generic;
using Constants;
using Infrastructure.AppRoot;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Services.ResourceLoader
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

		private T Load<T>(string name)
			where T : Object =>
			Resources.Load<T>(_resources[name]);
	}
}