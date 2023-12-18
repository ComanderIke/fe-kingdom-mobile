using System;
using Game.GameActors.Items.Gems;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    public class GemSlot
    {
        [SerializeReference]
        public Gem gem;

        private Relic boundTo;
        public GemSlot()
        {
            gem = null;
        }
        public bool IsEmpty()
        {
            return gem == null;
        }

        public void Bind(Relic relic)
        {
            boundTo = relic;
           
            gem.gemEffect.BindSkill(relic.user);
            gem.Rebind();
            gem.onSoulsIncreased += SoulsIncreased;
            Unit.UnitDied -= OnUnitDied;
            Unit.UnitDied += OnUnitDied;
        }

        void SoulsIncreased()
        {
            boundTo.user.UpdateStats();
        }
        public void Unbind(Relic relic)
        {
            boundTo =  null;
            gem.gemEffect.UnbindSkill(relic.user);
            gem.onSoulsIncreased -= SoulsIncreased;
            Unit.UnitDied -= OnUnitDied;
        }

        void OnUnitDied(Unit unit)
        {
            if (unit.KilledBy.Equals(boundTo.user))
            {
                gem.IncreaseSouls();
            }
        }
    }
}