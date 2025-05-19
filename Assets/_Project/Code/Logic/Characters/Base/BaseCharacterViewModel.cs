using Infrastructure.Roots.AppRoot.Services.Updater;
using Infrastructure.State.Entities.Player.Characteristics;
using Infrastructure.State.Entities.Player.Characteristics.Data;
using Infrastructure.State.Entities.Player.Characteristics.Proxy;
using R3;

namespace Logic.Characters.Base
{
	public abstract class BaseCharacterViewModel : ITick, IFixedTick
	{
		protected CharacterModel _model;
		
		public abstract void CreateModel(CharacteristicsDataProxy data);
		public abstract void Tick(float delta);
		public abstract void FixedTick(float delta);
	}
}