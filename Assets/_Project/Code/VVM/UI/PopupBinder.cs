using UnityEngine;
using UnityEngine.UI;

namespace VVM.UI
{
	public abstract class PopupBinder<T> : WindowBinder<T>
		where T : WindowViewModel
	{
		[SerializeField] private Button[] _bClose;

		protected virtual void Start()
		{
			foreach (var b in _bClose)
				b.onClick.AddListener(OnCloseButtonClicked);
		}

		protected virtual void OnDestroy()
		{
			foreach (var b in _bClose)
				b.onClick.RemoveListener(OnCloseButtonClicked);
		}

		protected virtual void OnCloseButtonClicked()
		{
			_viewModel.RequestClose();
		}
	}
}