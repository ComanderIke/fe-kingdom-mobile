using System;
using Game.GameActors.Units;

public interface IChooseTargetUI
{
    event Action OnBackClicked;
    void Show(Unit u, ITargetableObject targetableObject);
    void Hide();
}