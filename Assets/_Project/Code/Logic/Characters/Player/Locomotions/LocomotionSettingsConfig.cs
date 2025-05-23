using UnityEngine;

namespace Logic.Characters.Player.Locomotions
{
	[CreateAssetMenu(fileName = "config locomotin settings", menuName = "Configs/LocomotionSettingsConfig", order = 2)]
	public class LocomotionSettingsConfig : ScriptableObject
	{
		public Settings settings;
	}
}