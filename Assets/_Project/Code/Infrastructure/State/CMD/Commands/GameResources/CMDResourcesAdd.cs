using Infrastructure.State.CMD.Abstract;
using Infrastructure.State.GameResources;

namespace Infrastructure.State.CMD.Commands.GameResources
{
	public sealed class CMDResourcesAdd : ICommand
	{
		public readonly ResourceType resourceType;
		public readonly int amount;

		public CMDResourcesAdd(ResourceType resourceType, int amount)
		{
			this.resourceType = resourceType;
			this.amount = amount;
		}
	}
}