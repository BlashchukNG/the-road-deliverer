namespace Logic.Characters.Base.Locomotions.States
{
	public sealed class BaseState : IState
	{
		private readonly CharacterModel _model;

		public BaseState(CharacterModel model)
		{
			_model = model;
		}
		
		public void Exit()
		{
			
		}

		public IState Enter()
		{
			_model.LocomotionRuntimeData.previousRotation = _model.LocomotionReferences.Controller.transform.forward;
			return this;
		}

		public void Update(float delta)
		{
			
		}
	}
}