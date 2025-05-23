using System.Threading.Tasks;
using UnityEngine;

namespace Settings
{
	public class SettingsProvider : ISettingsProvider
	{
		private const string PATH_APP_SETTINGS = "settings/settings app";
		private const string PATH_GAME_SETTINGS = "settings/settings game";

		public AppSettings AppSettings { get; }
		public GameSettings GameSettings => _gameSettings;

		private GameSettings _gameSettings;


		public SettingsProvider()
		{
			AppSettings = Resources.Load<AppSettings>(PATH_APP_SETTINGS);
		}

		public Task<GameSettings> LoadGameSettingsAsync()
		{
			_gameSettings = Resources.Load<GameSettings>(PATH_GAME_SETTINGS);

			Debug.Log("Settings provider: loaded");

			return Task.FromResult(_gameSettings);
		}
	}
}