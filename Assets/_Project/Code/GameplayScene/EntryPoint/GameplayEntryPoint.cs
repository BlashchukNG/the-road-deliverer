using GameplayScene.UI;
using Infrastructure.AppRoot;
using Infrastructure.DI;
using Infrastructure.Services.AssetInstantiate;
using UnityEngine;

namespace GameplayScene.EntryPoint
{
	public class GameplayEntryPoint : MonoBehaviour
	{
		[SerializeField] private UIGameplayRootBinder _uiRootBinderPrefab;

		private DIContainer _diContainer;

		public void Run(DIContainer diContainer)
		{
			_diContainer = diContainer;
			var sceneUI = _diContainer.Resolve<IAssetInstantiateService>().GetInstance(_uiRootBinderPrefab);
			_diContainer.Resolve<UIRootView>().AttachSceneUI(sceneUI.gameObject);
		}
	}
}