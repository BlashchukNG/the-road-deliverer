using R3;
using UnityEngine;

namespace Infrastructure.State.Entities.Player
{
	public sealed class PlayerEntityProxy
	{
		public int Id { get; set; }
		public string TypeId { get; set; }
		public ReactiveProperty<Vector3> Position;
		public ReactiveProperty<Vector3> Rotation;

		public PlayerEntityProxy(PlayerEntity entity)
		{
			Id = entity.id;
			TypeId = entity.typeId;
			Position = new ReactiveProperty<Vector3>(entity.position);
			Rotation = new ReactiveProperty<Vector3>(entity.rotation);
			
			Position.Skip(1).Subscribe(value=>entity.position = value);
			Rotation.Skip(1).Subscribe(value=>entity.rotation = value);
		}
	}
}