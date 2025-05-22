using Infrastructure.Roots.AppRoot.Services.Updater;
using Infrastructure.State.Entities.Player.Characteristics.Proxy;
using Logic.Characters.Base.Locomotions;

namespace Logic.Characters.Base
{
	public abstract class BaseCharacterViewModel : ITick, IFixedTick
	{
		protected readonly ICharacterView _view;
		
		protected Locomotion _locomotion;
		protected CharacterModel _model;

		protected BaseCharacterViewModel(ICharacterView view)
		{
			_view = view;
		}

		protected virtual void CreateModel(CharacteristicsDataProxy data) => _model = new CharacterModel(data);

		public abstract void Tick(float delta);
		public abstract void FixedTick(float delta);
	}
}