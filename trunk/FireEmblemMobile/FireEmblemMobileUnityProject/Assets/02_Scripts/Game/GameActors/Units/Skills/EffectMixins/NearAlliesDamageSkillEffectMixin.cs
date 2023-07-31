using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.Grid;
using Game.Manager;
using Game.Map;
using Game.Mechanics;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/NearAlliesDamage", fileName = "NearAlliesDmgEffect")]
    public class NearAlliesDamageSkillEffectMixin : UnitTargetSkillEffectMixin
    {
        public int []dmgPerAlly;

        public int allyRange = 2;
        

        public override void Activate(Unit target, Unit caster, int level)
        {
            int allyCount = 0;
            var gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
            var tiles = gridSystem.Tiles;
            for (int x = -allyRange; x >= allyRange; x++)
            {
                for (int y = -allyRange; y>= allyRange; y++)
                {
                    if(x == 0&& y==0)
                        continue;
                    int tileX = x + caster.GridComponent.GridPosition.X;
                    int tileY = y + caster.GridComponent.GridPosition.Y;
                    
                    if (!gridSystem.IsOutOfBounds(tileX, tileY))
                    {
                        if (tiles[tileX, tileY].GridObject != null && tiles[tileX, tileY].GridObject is Unit unit)
                        {
                            if (unit.Faction.Id == caster.Faction.Id)
                            {
                                allyCount++;
                            }
                        }
                       
                    }
                        
                }
            }
           
            caster.Stats.BonusStatsFromEffects.Attack += dmgPerAlly[level] * allyCount;
            
        }

        public override void Deactivate(Unit user, Unit caster, int skillLevel)
        {
            throw new System.NotImplementedException();
        }

        public override List<EffectDescription> GetEffectDescription(int level)
        {
            return new List<EffectDescription>()
            {
                new EffectDescription("Dmg per ally: ", "" + dmgPerAlly[level],
                    "" + dmgPerAlly[level + 1])
            };
        }
        
    }
}