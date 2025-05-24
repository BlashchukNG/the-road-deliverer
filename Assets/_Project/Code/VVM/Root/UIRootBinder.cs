using ObservableCollections;
using R3;
using UnityEngine;

namespace VVM.Root
{
	public class UIRootBinder : MonoBehaviour
	{
		[SerializeField] private WindowsContainer _container;
		
		private readonly CompositeDisposable _subscriptions = new();
		
		public void Bind(UIRootViewModel viewModel)
		{
			_subscriptions.Add(viewModel.OpenedScreen.Subscribe(screen =>
			{
				_container.OpenScreen(screen);
			}));
			
			foreach (var popup in viewModel.OpenedPopups)
			{
				_container.OpenPopup(popup);
			}
			
			_subscriptions.Add(viewModel.OpenedPopups.ObserveAdd().Subscribe(e =>
			{
				_container.OpenPopup(e.Value);
			}));
			
			_subscriptions.Add(viewModel.OpenedPopups.ObserveRemove().Subscribe(e =>
			{
				_container.ClosePopup(e.Value);
			}));
			
			OnBind(viewModel);
		}
		
		protected virtual void OnBind(UIRootViewModel viewModel) {}

		private void OnDestroy()
		{
			_subscriptions.Dispose();
		}
	}
}