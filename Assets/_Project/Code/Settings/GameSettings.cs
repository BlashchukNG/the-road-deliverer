using System.Collections.Generic;
using Infrastructure.State.GameResources.Configs;
using Infrastructure.State.Root;
using Settings.Configs;
using UnityEngine;

namespace Settings
{
	[CreateAssetMenu(fileName = "settings game", menuName = "SETTINGS/new game settings", order = 0)]
	public class GameSettings : ScriptableObject
	{
		public GameState baseSaveFile;
		public List<ResourceConfig> configsResources;
		public PlayerConfig configPlayer;
	}
}