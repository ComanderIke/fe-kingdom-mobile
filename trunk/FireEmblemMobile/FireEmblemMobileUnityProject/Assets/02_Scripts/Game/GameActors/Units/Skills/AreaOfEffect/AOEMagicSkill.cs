using System;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.Grid;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    public class AOEMagicSkill : PositionTargetSkill
    {
        private DamageType damageType;
        public override void Activate(Unit user, Tile[,] tiles, int x, int y)
        {
            foreach (var pos in GetTargetPositions())
            {
                int xPosition = x + pos.x;
                int yPosition = y + pos.y;
                if (xPosition >= 0 && xPosition < tiles.GetLength(0) && yPosition >= 0 &&
                    yPosition < tiles.GetLength(1))
                {
                    if (tiles[xPosition, yPosition].GridObject != null)
                    {
                        Unit target = (Unit)tiles[xPosition, yPosition].GridObject;
                        Debug.Log("Position: "+xPosition+" "+yPosition);
                        Debug.Log("Deal " + (user.Stats.BaseAttributes.FAITH+power)+" Damage to : "+target);
                 
                        target.InflictFixedDamage(user,user.Stats.BaseAttributes.FAITH+power, DamageType.Faith);
                        if (!target.IsAlive())
                        {
                            target.Die();
                        }
                    }
                }
            }

            //Heall all Targets
            //make List of Target
            base.Activate(user,tiles, x, y);
        }

        public AOEMagicSkill(string name, string description, Sprite icon, GameObject animationObject, int tier,string[] upgradeDescriptions,int hpCost, int maxUses, int power, int range, int size, SkillTargetArea targetArea, bool rooted) : base(name, description, icon, animationObject, tier,upgradeDescriptions,hpCost,maxUses ,power, range, size, targetArea, EffectType.Bad,rooted)
        {
        }
    }
}