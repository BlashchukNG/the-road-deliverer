using UnityEngine;

namespace Infrastructure.State.GameResources.Configs
{
	[CreateAssetMenu(fileName = "config resouce", menuName = "SETTINGS/Configs/Resource/ResourceConfig", order = 0)]
	public class ResourceConfig : ScriptableObject
	{
		public ResourceData resource;
	}
}