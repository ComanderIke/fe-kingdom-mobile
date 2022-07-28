namespace Game.Grid
{
    public interface ITileEffectVisualRenderer
    {
        void ShowSwapable(Tile tile);
        void ShowAttackable(Tile tile);
        void Hide(Tile tile);
        
    }
}