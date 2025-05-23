using System;

namespace Infrastructure.State.GameResources
{
	[Serializable]
	public sealed class ResourceData
	{
		public string titleLocKey;
		public string descriptionLocKey;
		
		public ResourceType type;
		public int amount;
	}
}