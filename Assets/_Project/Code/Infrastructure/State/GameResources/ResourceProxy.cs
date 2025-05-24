using R3;

namespace Infrastructure.State.GameResources
{
	public sealed class ResourceProxy
	{
		public string TypeID { get; }
		public string TitleLocKey { get; }
		public string DescriptionLocKey { get; }

		public ResourceType Type { get; }
		public ReactiveProperty<int> Amount { get; }

		public readonly ResourceData origin;
		

		public ResourceProxy(ResourceData data)
		{
			origin = data;
			
			TypeID = data.typeId;
			TitleLocKey = data.titleLocKey;
			DescriptionLocKey = data.descriptionLocKey;
			Type = data.type;

			Amount = new ReactiveProperty<int>(data.amount);
			Amount.Skip(1).Subscribe(value => data.amount = value);
		}
	}
}