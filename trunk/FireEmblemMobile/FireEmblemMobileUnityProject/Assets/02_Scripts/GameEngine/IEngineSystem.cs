namespace GameEngine
{
    public interface IEngineSystem
    {
        void Init();
        void Deactivate();
        void Activate();
    }
}