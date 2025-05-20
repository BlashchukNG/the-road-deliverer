using System;
using Infrastructure.State.Entities.Player;

namespace Infrastructure.State.Root
{
	[Serializable]
	public sealed class GameState
	{
		public string version = "1.0";
		public PlayerEntity player;
	}
}