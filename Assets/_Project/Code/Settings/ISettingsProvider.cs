using System.Threading.Tasks;

namespace Settings
{
	public interface ISettingsProvider
	{
		AppSettings AppSettings { get; }
		GameSettings GameSettings { get; }

		Task<GameSettings> LoadGameSettingsAsync();
	}
}