using Game.GameActors.Units;

public interface IChooseTargetUI
{
    void Show(Unit u, ITargetableObject targetableObject);
    void Hide();
}