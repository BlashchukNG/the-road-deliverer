namespace Infrastructure.Roots.AppRoot.Updater
{
    public interface IUpdateService
    {
        void Add(IUpdatable updatable);
        void Remove(IUpdatable updatable);
        
        void Clear();
    }
}