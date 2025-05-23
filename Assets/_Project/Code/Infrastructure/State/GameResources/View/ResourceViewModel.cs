using R3;

namespace Infrastructure.State.GameResources.View
{
	public sealed class ResourceViewModel
	{
		public string TitleLocKey { get; }
		public string DescriptionLocKey { get; }
		public ResourceType Type { get; }
		public ReadOnlyReactiveProperty<int> Amount { get; }

		public ResourceViewModel(ResourceProxy resource)
		{
			TitleLocKey = resource.TitleLocKey;
			DescriptionLocKey = resource.DescriptionLocKey;
			Type = resource.Type;
			Amount = resource.Amount;
		}
	}
}