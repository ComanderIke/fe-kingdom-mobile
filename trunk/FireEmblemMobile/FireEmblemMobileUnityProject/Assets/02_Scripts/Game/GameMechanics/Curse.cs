using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills.Base;
using Game.GameActors.Units.Skills.EffectMixins;
using Game.Grid;
using Game.Manager;
using UnityEngine;

namespace Game.GameMechanics
{
   

    public class Curse : Skill
    {
        public Curse(string Name, string Description, Sprite icon, int tier,int maxLevel, List<PassiveSkillMixin> passiveMixins,CombatSkillMixin combatSkillMixin, List<ActiveSkillMixin> activeMixins, SkillTransferData data) : base(Name, Description, icon, tier,maxLevel, passiveMixins, combatSkillMixin, activeMixins,data)
        {
        }

        public void Spread()
        {
            var adjacentTiles = GridGameManager.Instance.GetSystem<GridSystem>().GetAdjacentTiles(owner.GridComponent.Tile);
            foreach (var adjacentTile in adjacentTiles)
            {
                if (adjacentTile.GridObject != null)
                {
                    Unit unit = ((Unit)adjacentTile.GridObject);
                    unit.ReceiveCurse((Curse)this.Clone());
                }
                 
            }
        }
        public override Skill Clone()
        {
            var newSkill = new Curse(Name, Description, Icon, Tier, maxLevel,passiveMixins,CombatSkillMixin,activeMixins, skillTransferData);
            newSkill.level = Level;
            return newSkill;
        }
    }
}