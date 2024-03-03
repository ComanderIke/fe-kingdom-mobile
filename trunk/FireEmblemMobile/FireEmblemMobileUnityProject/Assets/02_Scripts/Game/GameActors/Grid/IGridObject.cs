using Game.GameActors.Factions;
using Game.GameActors.Units.Components;
using Game.GameActors.Units.Interfaces;

namespace Game.GameActors.Grid
{
    public interface IGridObject
    {
        public GridComponent GridComponent { get; set; }
        public Faction Faction { get; }
        void SetAttackTarget(bool selected);
        bool IsEnemy(IGridActor selectedActor);
    }
}