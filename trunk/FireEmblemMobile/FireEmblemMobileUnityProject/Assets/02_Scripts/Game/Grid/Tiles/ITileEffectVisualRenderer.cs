namespace Game.Grid.Tiles
{
    public interface ITileEffectVisualRenderer
    {
        void ShowSwapable(Tile tile);
        void ShowAttackable(Tile tile);
        void Hide(Tile tile);
        
    }
}