using Infrastructure.Roots.AppRoot.Services.AssetInstantiate;
using Infrastructure.Roots.GarageScene.ViewModels;
using Infrastructure.State;
using Logic.Characters.Player;
using Logic.UserCamera;
using UnityEngine;

namespace Infrastructure.Roots.GarageScene.Views
{
	public sealed class WorldGarageRootBinder : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private CameraController _camera;
		[SerializeField] private Transform _layerPlayer;

		private PlayerBinder _player;

		public void Bind(WorldGarageRootViewModel rootViewModel)
		{
			_camera = FindFirstObjectByType<CameraController>();
			_camera.SetInput(rootViewModel.DIContainer);
			CreatePlayer(rootViewModel);
		}

		private void CreatePlayer(WorldGarageRootViewModel rootViewModel)
		{
			var assetInstantiateService = rootViewModel.DIContainer.Resolve<IAssetInstantiateService>();
			var playerState = rootViewModel.DIContainer.Resolve<IGameStateProvider>().GameState.Player;

			_player = assetInstantiateService.GetInstance(Resources.Load<PlayerBinder>(rootViewModel.PrefabPlayer), _layerPlayer, playerState.Position.Value, 
				Quaternion.Euler(playerState.Rotation.Value));
			
			_player.Bind(rootViewModel.PlayerViewModel, _camera);
			
			_camera.SetFollowTarget(_player.CharacterController.transform);
		}
	}
}