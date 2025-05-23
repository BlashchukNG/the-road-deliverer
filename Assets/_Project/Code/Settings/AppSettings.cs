using Infrastructure.State.Root;
using UnityEngine;

namespace Settings
{
	[CreateAssetMenu(fileName = "settings app", menuName = "SETTINGS/new app settings", order = 0)]
	public class AppSettings : ScriptableObject
	{
		public GameSettingsState settings;
	}
}