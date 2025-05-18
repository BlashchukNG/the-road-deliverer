using Infrastructure.AppRoot;
using Infrastructure.DI;
using Infrastructure.Services.AssetInstantiate;
using MainMenu.UI;
using UnityEngine;

namespace MainMenu.EntryPoint
{
	public class MainMenuEntryPoint : MonoBehaviour
	{
		[SerializeField] private UIMainMenuRootBinder _uiRootBinderPrefab;

		private DIContainer _diContainer;

		public void Run(DIContainer diContainer)
		{
			_diContainer = diContainer;
			var sceneUI = _diContainer.Resolve<IAssetInstantiateService>().GetInstance(_uiRootBinderPrefab);
			_diContainer.Resolve<UIRootView>().AttachSceneUI(sceneUI.gameObject);
		}
	}
}