using Infrastructure.State.Root;
using UnityEngine;

namespace Infrastructure.State
{
	[CreateAssetMenu(fileName = "config base save", menuName = "Configs/SaveFileConfig", order = 1)]
	public class SaveFileConfig : ScriptableObject
	{
		public GameState state;
	}
}