using Infrastructure.DI;
using Infrastructure.Roots.AppRoot.Services.ResourceLoader;
using Infrastructure.State.Root;
using R3;
using UnityEngine;

namespace Infrastructure.State
{
	public sealed class PlayerPrefsGameStateProvider : IGameStateProvider
	{
		private const string GAME_STATE_KEY = nameof(GAME_STATE_KEY);
		private const string GAME_SETTINGS_STATE_KEY = nameof(GAME_SETTINGS_STATE_KEY);

		public GameStateProxy GameState { get; private set; }
		public GameSettingsStateProxy GameSettingsState { get; private set; }

		private readonly SaveFileConfig _saveFileConfig;

		private GameState _gameStateOrigin;
		private GameSettingsState _gameSettingsStateOrigin;

		public PlayerPrefsGameStateProvider(DIContainer diContainer)
		{
			_saveFileConfig = diContainer.Resolve<IResourceLoaderService>().GetBaseSaveFile();
		}

		public Observable<GameStateProxy> LoadGameState()
		{
			if (!PlayerPrefs.HasKey(GAME_STATE_KEY))
			{
				GameState = CreateGameStateFromSettings();
				SaveGameState();
			}
			else
			{
				var json = PlayerPrefs.GetString(GAME_STATE_KEY);
				_gameStateOrigin = JsonUtility.FromJson<GameState>(json);
				GameState = new GameStateProxy(_gameStateOrigin);

				Debug.Log($"GameStateProvider: state loaded: {json}");
			}

			return Observable.Return(GameState);
		}

		private GameStateProxy CreateGameStateFromSettings()
		{
			_gameStateOrigin = _saveFileConfig.state;

			Debug.Log($"GameStateProvider: state created from settings: {JsonUtility.ToJson(_gameStateOrigin, true)}");

			return new GameStateProxy(_gameStateOrigin);
		}

		public Observable<bool> SaveGameState()
		{
			var json = JsonUtility.ToJson(_gameStateOrigin, true);
			PlayerPrefs.SetString(GAME_STATE_KEY, json);

			Debug.Log($"GameStateProvider: state saved: {json}");

			return Observable.Return(true);
		}

		public Observable<bool> ResetGameState()
		{
			GameState = CreateGameStateFromSettings();
			SaveGameState();

			return Observable.Return(true);
		}

		public Observable<GameSettingsStateProxy> LoadGameSettingsState()
		{
			if (!PlayerPrefs.HasKey(GAME_SETTINGS_STATE_KEY))
			{
				GameSettingsState = CreateGameSettingsStateFromSettings();
				SaveGameState();
			}
			else
			{
				var json = PlayerPrefs.GetString(GAME_SETTINGS_STATE_KEY);
				_gameSettingsStateOrigin = JsonUtility.FromJson<GameSettingsState>(json);
				GameSettingsState = new GameSettingsStateProxy(_gameSettingsStateOrigin);

				Debug.Log($"GameStateProvider: settings state loaded: {json}");
			}

			return Observable.Return(GameSettingsState);
		}

		private GameSettingsStateProxy CreateGameSettingsStateFromSettings()
		{
			_gameSettingsStateOrigin = new()
			{
				volumeMusic = 1f,
				volumeSFX = 1f
			};

			Debug.Log($"GameStateProvider: settings state created from settings: {JsonUtility.ToJson(_gameSettingsStateOrigin, true)}");

			return new GameSettingsStateProxy(_gameSettingsStateOrigin);
		}

		public Observable<bool> SaveGameSettingsState()
		{
			var json = JsonUtility.ToJson(_gameSettingsStateOrigin, true);
			PlayerPrefs.SetString(GAME_SETTINGS_STATE_KEY, json);

			Debug.Log($"GameStateProvider: settings state saved: {json}");

			return Observable.Return(true);
		}

		public Observable<bool> ResetGameSettingsState()
		{
			GameSettingsState = CreateGameSettingsStateFromSettings();
			SaveGameSettingsState();

			return Observable.Return(true);
		}
	}
}