using Infrastructure.DI;
using Infrastructure.State;
using Infrastructure.State.Entities.Player.Characteristics;
using Infrastructure.State.Entities.Player.Characteristics.Data;
using Infrastructure.State.Entities.Player.Characteristics.Proxy;
using Logic.Characters.Base;
using R3;

namespace Logic.Characters.Player
{
	public sealed class PlayerViewModel : BaseCharacterViewModel
	{
		private readonly PlayerView _view;

		public PlayerViewModel(PlayerView view, DIContainer container)
		{
			_view = view;
			CreateModel(container.Resolve<IGameStateProvider>().GameState.Player.CharacteristicsData);
		}

		public override void CreateModel(CharacteristicsDataProxy data)
		{
			_model = new CharacterModel(data);
		}

		public override void Tick(float delta)
		{
			
		}

		public override void FixedTick(float delta)
		{
			
		}
	}
}