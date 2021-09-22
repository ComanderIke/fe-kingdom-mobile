using System;
using Game.Grid;

public interface IPlayerPhaseUI
{
    
    void Show(int turnCount);
    void Hide();
    public void ShowTileInfo(Tile selectedTile);
    public void HideTileInfo();
    public void SubscribeOnBackClicked(Action action);
    public void UnsubscribeOnBackClicked(Action action);
}