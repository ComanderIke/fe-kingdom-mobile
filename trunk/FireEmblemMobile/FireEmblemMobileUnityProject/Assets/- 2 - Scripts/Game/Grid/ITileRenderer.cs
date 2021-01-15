namespace Game.Grid
{
    public interface ITileRenderer
    {
        void Reset();
        void AttackVisual();
        void AllyVisual();
        void MoveVisual();
        void ActiveAttackVisual();
        void ActiveMoveVisual();
        void StandOnVisual();
        void SetVisualStyle(int id);
    }
}