using System;
using UnityEngine;

namespace Infrastructure.State.Entities.Abstract
{
	[Serializable]
	public abstract class Entity
	{
		public int id;
		public string typeId;
		public Vector3 position;
		public Vector3 rotation;
	}
}