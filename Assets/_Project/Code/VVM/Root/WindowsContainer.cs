using System.Collections.Generic;
using UnityEngine;
using VVM.UI;

namespace VVM.Root
{
	public sealed class WindowsContainer : MonoBehaviour
	{
		[SerializeField] private Transform _containerScreens;
		[SerializeField] private Transform _containerPopups;

		private readonly Dictionary<WindowViewModel, IWindowBinder> _openedPopups = new();
		private IWindowBinder _openedScreen;


		public void OpenScreen(WindowViewModel viewModel)
		{
			if (viewModel == null) return;

			_openedScreen?.Close();

			var prefabPath = GetScreenPrefabPath(viewModel);
			var prefab = Resources.Load<GameObject>(prefabPath);
			var createdPopup = Instantiate(prefab, _containerScreens);
			var binder = createdPopup.GetComponent<IWindowBinder>();

			binder.Bind(viewModel);
			_openedScreen = binder;
		}

		public void OpenPopup(WindowViewModel viewModel)
		{
			var prefabPath = GetPopupPrefabPath(viewModel);
			var prefab = Resources.Load<GameObject>(prefabPath);
			var createdPopup = Instantiate(prefab, _containerPopups);
			var binder = createdPopup.GetComponent<IWindowBinder>();

			binder.Bind(viewModel);
			_openedPopups.Add(viewModel, binder);
		}

		public void ClosePopup(WindowViewModel viewModel)
		{
			var binder = _openedPopups[viewModel];
			binder?.Close();
			_openedPopups.Remove(viewModel);
		}

		private string GetPopupPrefabPath(WindowViewModel viewModel) => $"prefabs/ui/popups/{viewModel.Id}";
		private string GetScreenPrefabPath(WindowViewModel viewModel) => $"prefabs/ui/screens/{viewModel.Id}";
	}
}