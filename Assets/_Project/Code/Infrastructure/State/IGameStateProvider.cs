using Infrastructure.State.Root;
using R3;

namespace Infrastructure.State
{
	public interface IGameStateProvider
	{
		public GameStateProxy GameState { get; }
		public GameSettingsStateProxy GameSettingsState { get; }
		
		public Observable<GameStateProxy> LoadGameState();
		public Observable<bool> SaveGameState();
		public Observable<bool> ResetGameState();
		
		public Observable<GameSettingsStateProxy> LoadGameSettingsState();
		public Observable<bool> SaveGameSettingsState();
		public Observable<bool> ResetGameSettingsState();
	}
}