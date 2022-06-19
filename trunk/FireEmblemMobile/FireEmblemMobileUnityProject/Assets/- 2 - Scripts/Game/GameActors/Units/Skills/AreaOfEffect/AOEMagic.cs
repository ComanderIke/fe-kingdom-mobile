using System;
using Game.Grid;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/AOE_Damage_Magic", fileName = "AOEDamageMagic")]
    public class AOEMagic : PositionTargetSkill
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
                        Debug.Log("Deal " + (user.Stats.Attributes.FAITH+power)+" Damage to : "+target);
                 
                        target.InflictFixedFaithDamage(user.Stats.Attributes.FAITH+power);
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
    }
}