using UnityEngine;

namespace Logic.Characters.Base.Locomotions
{
	[CreateAssetMenu(fileName = "config locomotin settings", menuName = "Configs/LocomotionSettingsConfig", order = 2)]
	public class LocomotionSettingsConfig : ScriptableObject
	{
		public LocomotionSettings settings;
	}
}