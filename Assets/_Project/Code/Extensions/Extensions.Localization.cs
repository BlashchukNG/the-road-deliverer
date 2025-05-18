using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Extensions
{
	public static partial class Extensions
	{
		public static string Localize(this string str, string localizationTable)
		{
			if (LocalizationSettings.StringDatabase.GetTable(localizationTable) == null)
				Debug.LogError($"StringExtensions | Localization table {localizationTable} does not exists! Check adressables.");
			return LocalizationSettings.StringDatabase.GetLocalizedString(localizationTable, str);
		}
	}
}