using Infrastructure.State.Entities.Player.Characteristics.Proxy;
using R3;
using UnityEngine;

namespace Infrastructure.State.Entities.Player
{
	public sealed class PlayerEntityProxy
	{
		#region Base

		public int Id { get; set; }
		public string TypeId { get; set; }

		public ReactiveProperty<Vector3> Position;
		public ReactiveProperty<Vector3> Rotation;

		#endregion

		public CharacteristicsDataProxy CharacteristicsData { get; }

		public PlayerEntityProxy(PlayerEntity entity)
		{
			Id = entity.id;
			TypeId = entity.typeId;

			Position = new ReactiveProperty<Vector3>(entity.position);
			Position.Skip(1).Subscribe(value => entity.position = value);
			Rotation = new ReactiveProperty<Vector3>(entity.rotation);
			Rotation.Skip(1).Subscribe(value => entity.rotation = value);

			CharacteristicsData = new CharacteristicsDataProxy(entity.characteristics);
		}
	}
}