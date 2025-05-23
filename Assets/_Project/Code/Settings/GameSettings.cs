using Infrastructure.State.Root;
using Logic.Characters.Player.Locomotions;
using UnityEngine;

namespace Settings
{
	[CreateAssetMenu(fileName = "settings game", menuName = "SETTINGS/new game settings", order = 0)]
	public class GameSettings : ScriptableObject
	{
		public GameState baseSaveFile;
		public PlayerConfig configPlayer;
	}
}