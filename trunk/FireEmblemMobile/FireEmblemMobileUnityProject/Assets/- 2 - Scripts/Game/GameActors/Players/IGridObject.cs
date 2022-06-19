using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.Grid;

namespace Game.GameActors.Players
{
    public interface IGridObject
    {
        public GridComponent GridComponent { get; set; }
        public Faction Faction { get; }
        void SetAttackTarget(bool selected);
        bool IsEnemy(IGridActor selectedActor);
    }
}