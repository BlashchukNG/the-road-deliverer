using System;
using System.Collections.Generic;
using Infrastructure.DI;
using Infrastructure.State.CMD;
using Infrastructure.State.CMD.Commands.GameResources;
using Infrastructure.State.GameResources.View;
using ObservableCollections;
using R3;

namespace Infrastructure.State.GameResources.Services
{
	public sealed class ResourcesService
	{
		public readonly ObservableList<ResourceViewModel> Resources = new();

		private readonly CommandProcessor _cmd;
		private readonly Dictionary<ResourceType, ResourceViewModel> _resourcesMap = new();

		public ResourcesService(DIContainer diContainer)
		{
			_cmd = diContainer.Resolve<CommandProcessor>();
			var state = diContainer.Resolve<IGameStateProvider>();
			state.GameState.Resources.ForEach(CreateResourceViewModel);
			state.GameState.Resources.ObserveAdd().Subscribe(e => CreateResourceViewModel(e.Value));
			state.GameState.Resources.ObserveRemove().Subscribe(e => RemoveResourceViewModel(e.Value));
		}

		public bool AddResource(ResourceType type, int amount) => _cmd.ProcessCommand(new CMDResourcesAdd(type, amount));
		public bool TrySpendResource(ResourceType type, int amount) => _cmd.ProcessCommand(new CMDResourcesSpend(type, amount));

		public bool IsEnoughResource(ResourceType type, int amount)
		{
			if (_resourcesMap.TryGetValue(type, out ResourceViewModel resourceViewModel))
				return resourceViewModel.Amount.CurrentValue >= amount;

			return false;
		}

		public Observable<int> ObserveResourceAmount(ResourceType type)
		{
			if (_resourcesMap.TryGetValue(type, out ResourceViewModel resourceViewModel))
				return resourceViewModel.Amount;

			throw new Exception($"ResourcesService: resource of type {type} doesn't exist");
		}

		private void CreateResourceViewModel(ResourceProxy resource)
		{
			var viewModel = new ResourceViewModel(resource);
			_resourcesMap.Add(resource.Type, viewModel);
			Resources.Add(viewModel);
		}

		private void RemoveResourceViewModel(ResourceProxy resource)
		{
			if (_resourcesMap.TryGetValue(resource.Type, out var viewModel))
			{
				Resources.Remove(viewModel);
				_resourcesMap.Remove(resource.Type);
			}
		}
	}
}