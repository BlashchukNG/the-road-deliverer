using R3;

namespace Infrastructure.State.Root
{
	public sealed class GameSettingsStateProxy
	{
		public ReactiveProperty<float> VolumeMusic { get; }
		public ReactiveProperty<float> VolumeSFX { get; }

		public GameSettingsStateProxy(GameSettingsState state)
		{
			VolumeMusic = new ReactiveProperty<float>(state.volumeMusic);
			VolumeSFX = new ReactiveProperty<float>(state.volumeSFX);

			VolumeMusic.Skip(1).Subscribe(value => state.volumeMusic = value);
			VolumeSFX.Skip(1).Subscribe(value => state.volumeSFX = value);
		}
	}
}