using System.Collections.Generic;
using Infrastructure.Roots.AppRoot.Services.UserUnput;

namespace Logic.Characters.Base.Locomotions.States
{
	public sealed class StateMachine
	{
		private readonly CharacterModel _model;
		private readonly InputCalculator _inputCalculator;
		private readonly Calculator _calculator;
		private readonly Checker _checker;
		private readonly AnimatorVariablesUpdater _animatorVariablesUpdater;

		private Dictionary<AnimationState, IState> _states = new();
		private IState _currentState;

		public StateMachine(CharacterModel model, IUserInputService input)
		{
			_model = model;
			_checker = new Checker(_model);
			_calculator = new Calculator(_model);
			_inputCalculator = new InputCalculator(_model, input);
			_animatorVariablesUpdater = new AnimatorVariablesUpdater(_model);

			_states[AnimationState.Base] = new BaseState(_model);
			_states[AnimationState.Locomotion] = new LocomotionState(this, _model, input, _checker, _calculator, _inputCalculator, _animatorVariablesUpdater);
			_states[AnimationState.Crouch] = new CrouchState(this, _model, input, _checker, _calculator, _inputCalculator, _animatorVariablesUpdater);
			_states[AnimationState.Jump] = new JumpState(this, _model, _checker, _calculator, _inputCalculator, _animatorVariablesUpdater);
			_states[AnimationState.Fall] = new FallState(this, _model, _checker, _calculator, _inputCalculator, _animatorVariablesUpdater);
		}

		public StateMachine SwitchState(AnimationState newState)
		{
			_currentState?.Exit();
			_currentState = _states[newState].Enter();
			return this;
		}

		public void Update(float delta)
		{
			_currentState.Update(delta);
		}
	}
}