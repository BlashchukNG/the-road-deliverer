using Infrastructure.Roots.AppRoot.Services.UserInput;
using Logic.Characters.Base.Locomotions.States;

namespace Logic.Characters.Base.Locomotions
{
	public sealed class Locomotion
	{
		private readonly CharacterModel _model;
		private readonly StateMachine _stateMachine;

		public Locomotion(CharacterModel model, IUserInputService input)
		{
			_model = model;
			_stateMachine = new StateMachine(_model, input).SwitchState(AnimationState.Locomotion);
		}

		public void Update(float delta)
		{
			_stateMachine.Update(delta);
			_model.Entity.Position.Value = _model.LocomotionReferences.Controller.transform.position;
			_model.Entity.Rotation.Value = _model.LocomotionReferences.Controller.transform.rotation.eulerAngles;
		}
	}
}