namespace Game.Grid
{
    public interface ITileRenderer
    {
        void Reset();
        void AttackVisual();
        void AllyVisual();
        void MoveVisual();
        void StandOnVisual();
    }
}