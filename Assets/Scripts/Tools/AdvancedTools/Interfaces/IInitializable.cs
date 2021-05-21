namespace Tools.Interfaces
{
    public interface IInitializable: IPlayable
    {
        void Initialize();
        void DeInitialize();
    }
}