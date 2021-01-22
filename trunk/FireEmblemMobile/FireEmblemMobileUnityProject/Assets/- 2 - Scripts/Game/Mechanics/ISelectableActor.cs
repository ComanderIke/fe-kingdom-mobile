using Game.GameActors.Units;

namespace Game.Mechanics
{
    public interface ISelectableActor :IGridActor
    {
        void SetAttackTarget(bool selected);

    }
}