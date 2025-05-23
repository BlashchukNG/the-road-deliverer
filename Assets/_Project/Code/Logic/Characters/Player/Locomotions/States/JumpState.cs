using Infrastructure.Roots.AppRoot.Services.UserInput;
using UnityEngine;

namespace Logic.Characters.Player.Locomotions.States
{
	public sealed class JumpState : IState
	{
		private readonly StateMachine _stateMachine;
		private readonly IUserInputService _input;
		private readonly PlayerBehavior _model;
		private readonly Checker _checker;
		private readonly Calculator _calculator;
		private readonly InputCalculator _inputCalculator;
		private readonly AnimatorVariablesUpdater _animatorVariablesUpdater;

		public JumpState
		(StateMachine stateMachine,
			PlayerBehavior model,
			Checker checker, Calculator calculator,
			InputCalculator inputCalculator,
			AnimatorVariablesUpdater animatorVariablesUpdater)
		{
			_stateMachine = stateMachine;
			_model = model;
			_checker = checker;
			_calculator = calculator;
			_inputCalculator = inputCalculator;
			_animatorVariablesUpdater = animatorVariablesUpdater;
		}

		public void Exit()
		{
			_model.References.Animator.SetBool(_model.AnimationsVariables.isJumpingAnimHash, false);
		}

		public IState Enter()
		{
			_model.References.Animator.SetBool(_model.AnimationsVariables.isJumpingAnimHash, true);
			_model.RuntimeData.isSliding = false;
			_model.RuntimeData.velocity = new Vector3(_model.RuntimeData.velocity.x,
				_model.Settings.jumpForce, _model.RuntimeData.velocity.z);

			return this;
		}

		public void Update(float delta)
		{
			ApplyGravity();

			if (_model.RuntimeData.velocity.y <= 0f)
			{
				_model.References.Animator.SetBool(_model.AnimationsVariables.isJumpingAnimHash, false);
				_stateMachine.SwitchState(AnimationState.Fall);
			}

			_checker.GroundCheck();

			_calculator.CalculateRotationalAdditives(delta, false, _model.Settings.enableHeadTurn,
				_model.Settings.enableBodyTurn);
			_calculator.CalculateMoveDirection();
			_calculator.CalculateFaceMoveDirection(delta);
			_model.References.Controller.Move(_model.RuntimeData.velocity * delta);
			_animatorVariablesUpdater.UpdateAnimatorController();
		}

		private void ApplyGravity()
		{
			if (_model.RuntimeData.velocity.y > Physics.gravity.y)
				_model.RuntimeData.velocity.y += Physics.gravity.y * _model.Settings.gravityMultiplier * Time.deltaTime;
		}
	}
}