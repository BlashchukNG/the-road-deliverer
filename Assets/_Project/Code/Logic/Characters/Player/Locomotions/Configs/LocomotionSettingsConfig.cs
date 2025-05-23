using UnityEngine;

namespace Logic.Characters.Player.Locomotions.Configs
{
	[CreateAssetMenu(fileName = "config locomotin settings", menuName = "SETTINGS/Configs/Player/LocomotionSettingsConfig", order = 0)]
	public class LocomotionSettingsConfig : ScriptableObject
	{
		public Settings settings;
	}
}