namespace Infrastructure.Roots.AppRoot.Updater
{
    public interface ILateTick
    {
        void LateTick(float delta);
    }
}