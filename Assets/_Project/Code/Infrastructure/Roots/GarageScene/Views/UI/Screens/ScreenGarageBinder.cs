using System;
using System.Collections.Generic;
using Infrastructure.State.GameResources;
using Infrastructure.State.GameResources.View;
using ObservableCollections;
using R3;
using UnityEngine;
using VVM.UI;

namespace Infrastructure.Roots.GarageScene.Views.UI.Screens
{
	public sealed class ScreenGarageBinder : WindowBinder<ScreenGarageViewModel>
	{
		[SerializeField] public Transform _rootResouces;
		
		[SerializeField] public GarageButton _bGoToMainMenuScene;
		[SerializeField] public GarageButton _bGoToGameplaySceneScene;

		
		private readonly CompositeDisposable _subscriptions = new();
		private readonly Dictionary<ResourceType, ResourceBinder> _resources = new();

		protected override void OnBind(ScreenGarageViewModel viewModel)
		{
			base.OnBind(viewModel);
			
			foreach (var resource in viewModel.Resources)
			{
				CreateResource(resource);
			}
			
			_subscriptions.Add(_viewModel.Resources.ObserveAdd().Subscribe(e =>
			{
				CreateResource(e.Value);
			}));
			
			_subscriptions.Add(_viewModel.Resources.ObserveRemove().Subscribe(e =>
			{
				RemoveResource(e.Value);
			}));
		}

		private void CreateResource(ResourceViewModel resource)
		{
			var prefab = Resources.Load<ResourceBinder>($"prefabs/ui/game resources/{resource.TypeID}");
			if (prefab == null) throw new Exception($"Resource prefab {resource.TypeID} not found");
			var binder = Instantiate(prefab, _rootResouces);
			binder.Bind(resource);
			_resources[resource.Type] = binder;
		}

		private void RemoveResource(ResourceViewModel resource)
		{
			if (_resources.TryGetValue(resource.Type, out var binder))
			{
				Destroy(binder.gameObject);
				_resources.Remove(resource.Type);
			}
		}

		private void OnEnable()
		{
			_bGoToMainMenuScene.Button.onClick.AddListener(OnGoToMainMenuClicked);
			_bGoToGameplaySceneScene.Button.onClick.AddListener(OnGoToGameplaySceneClicked);
		}

		private void OnDisable()
		{
			_bGoToMainMenuScene.Button.onClick.RemoveListener(OnGoToMainMenuClicked);
			_bGoToGameplaySceneScene.Button.onClick.RemoveListener(OnGoToGameplaySceneClicked);
		}

		private void OnGoToMainMenuClicked()
		{
			_viewModel.RequestGoToMainMenuScene();
		}

		private void OnGoToGameplaySceneClicked()
		{
			_viewModel.RequestGoToGameplayScene();
		}

		private void OnDestroy() => _subscriptions.Dispose();
	}
}