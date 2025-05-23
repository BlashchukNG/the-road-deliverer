using System.Linq;
using Infrastructure.DI;
using Infrastructure.State.CMD.Abstract;
using Infrastructure.State.CMD.Commands.GameResources;
using Infrastructure.State.GameResources;
using Settings;
using UnityEngine;

namespace Infrastructure.State.CMD.Handlers.GameResources
{
	public sealed class CMDResourcesSpendHandler : ICommandHandler<CMDResourcesSpend>
	{
		private readonly DIContainer _diContainer;
		private readonly IGameStateProvider _dameStateProxy;

		public CMDResourcesSpendHandler(DIContainer diContainer)
		{
			_diContainer = diContainer;
			_dameStateProxy = diContainer.Resolve<IGameStateProvider>();
		}

		public bool Handle(CMDResourcesSpend command)
		{
			var requiredType = command.resourceType;
			var requiredResource = _dameStateProxy.GameState.Resources.FirstOrDefault(r => r.Type == requiredType);

			if (requiredResource == null)
			{
				Debug.LogError($"CMDResourcesSpendHandler: Could not find required resource type {requiredType}");
				return false;
			}

			if (requiredResource.Amount.Value < command.amount)
			{
				Debug.LogError($"CMDResourcesSpendHandler: Trying to spend more than existed |{command.amount} to resource {requiredType}|");
				return false;
			}

			requiredResource.Amount.Value -= command.amount;

			return true;
		}

		private ResourceProxy CreateNewResource(ResourceType requiredType)
		{
			var config = _diContainer.Resolve<ISettingsProvider>().GameSettings.configsResources.First(r => r.resource.type == requiredType);
			var newResource = new ResourceProxy(config.resource.Clone() as ResourceData);
			
			_dameStateProxy.GameState.Resources.Add(newResource);
			
			return newResource;
		}
	}
}