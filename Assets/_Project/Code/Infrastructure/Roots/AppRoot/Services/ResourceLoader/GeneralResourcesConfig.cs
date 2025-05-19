using System;
using UnityEngine;

namespace Infrastructure.Roots.AppRoot.Services.ResourceLoader
{
	[CreateAssetMenu(fileName = "config general resources", menuName = "Configs/GeneralResourcesConfig", order = 0)]
	public class GeneralResourcesConfig : ScriptableObject
	{
		public ResourcePath[] resources;
	}

	[Serializable]
	public struct ResourcePath
	{
		public string name, path;
	}
}