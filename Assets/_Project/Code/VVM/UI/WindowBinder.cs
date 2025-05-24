using UnityEngine;

namespace VVM.UI
{
	public abstract class WindowBinder<T> : MonoBehaviour, IWindowBinder
		where T : WindowViewModel
	{
		protected T _viewModel;

		public void Bind(WindowViewModel viewModel)
		{
			_viewModel = (T)viewModel;
			OnBind(_viewModel);
		}

		public virtual void Close()
		{
			Destroy(gameObject);
		}

		protected virtual void OnBind(T viewModel)
		{
		}
	}
}