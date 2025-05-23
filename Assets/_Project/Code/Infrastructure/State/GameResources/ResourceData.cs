using System;

namespace Infrastructure.State.GameResources
{
	[Serializable]
	public sealed class ResourceData : ICloneable
	{
		public string titleLocKey;
		public string descriptionLocKey;
		
		public ResourceType type;
		public int amount;
		
		public object Clone() => MemberwiseClone();
	}
}