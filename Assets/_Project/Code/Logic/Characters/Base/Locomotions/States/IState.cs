namespace Logic.Characters.Base.Locomotions.States
{
	public interface IState
	{
		public void Exit();
		public IState Enter();
		public void Update(float delta);
	}
}