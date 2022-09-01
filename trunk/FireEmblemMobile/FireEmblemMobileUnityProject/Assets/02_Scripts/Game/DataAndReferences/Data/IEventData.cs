namespace Game.GameResources
{
    public interface IEventData
    {
        RandomEvent GetRandomEvent(int tier);
        RandomEvent GetSpecialEvent(int index);
    }
}