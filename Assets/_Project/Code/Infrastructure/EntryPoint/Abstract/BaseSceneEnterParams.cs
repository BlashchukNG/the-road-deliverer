namespace Code.Infrastructure.EntryPoint.Abstract
{
	public abstract class BaseSceneEnterParams
	{
		public string SceneName { get; }

		protected BaseSceneEnterParams(string sceneName)
		{
			SceneName = sceneName;
		}

		public T As<T>()
			where T : BaseSceneEnterParams =>
			(T)this;
	}
}