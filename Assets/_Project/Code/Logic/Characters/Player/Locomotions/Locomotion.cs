using Infrastructure.Roots.AppRoot.Services.UserInput;
using Logic.Characters.Player.Locomotions.States;

namespace Logic.Characters.Player.Locomotions
{
	public sealed class Locomotion
	{
		private readonly PlayerBehavior _model;
		private readonly StateMachine _stateMachine;

		public Locomotion(PlayerBehavior model, IUserInputService input)
		{
			_model = model;
			_stateMachine = new StateMachine(_model, input).SwitchState(AnimationState.Locomotion);
		}

		public void Update(float delta)
		{
			_stateMachine.Update(delta);
			_model.Entity.Position.Value = _model.References.Controller.transform.position;
			_model.Entity.Rotation.Value = _model.References.Controller.transform.rotation.eulerAngles;
		}
	}
}