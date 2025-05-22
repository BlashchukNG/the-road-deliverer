using Infrastructure.DI;
using Infrastructure.Roots.AppRoot.Services.Updater;
using Infrastructure.State.Entities.Player.Characteristics.Proxy;
using Logic.Characters.Base.Locomotions;

namespace Logic.Characters.Base
{
	public abstract class BaseCharacterViewModel : ITick, IFixedTick
	{
		protected readonly ICharacterView _view;
		protected readonly DIContainer _diContainer;

		protected CharacterModel _model;

		protected BaseCharacterViewModel(ICharacterView view, DIContainer diContainer)
		{
			_view = view;
			_diContainer = diContainer;
		}

		protected abstract void CreateModel();

		public abstract void Tick(float delta);
		public abstract void FixedTick(float delta);
	}
}