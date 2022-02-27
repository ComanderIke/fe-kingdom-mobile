using System;
using Game.GameActors.Units;
using Game.Grid;

public interface IPlayerPhaseUI
{
    
    void Show(int turnCount);
    void Hide();
    public void ShowTileInfo(Tile selectedTile);
    public void HideTileInfo();
    public void SubscribeOnBackClicked(Action action);
    public void UnsubscribeOnBackClicked(Action action);
    public void SubscribeOnCharacterCircleClicked(Action<Unit> onCharacterCircleClicked);
    void UnsubscribeOnCharacterCircleClicked(Action<Unit> onCharacterCircleClicked);
}