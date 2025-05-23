namespace Logic.Characters.Player.Locomotions.States
{
	public sealed class BaseState : IState
	{
		private readonly PlayerBehavior _model;

		public BaseState(PlayerBehavior model)
		{
			_model = model;
		}
		
		public void Exit()
		{
			
		}

		public IState Enter()
		{
			_model.RuntimeData.previousRotation = _model.References.Controller.transform.forward;
			return this;
		}

		public void Update(float delta)
		{
			
		}
	}
}