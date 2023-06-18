﻿using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [System.Serializable]
    public class PositionHealSkill : PositionTargetSkill
    {
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
                        ((Unit)tiles[xPosition, yPosition].GridObject).Heal(power);
                    }
                }
            }

            //Heall all Targets
            //make List of Target
            base.Activate(user,tiles, x, y);
        }

        public PositionHealSkill(string name, string description, Sprite icon, GameObject animationObject, int cooldown, int tier,string[] upgradeDescriptions, int hpCost, int maxUses,int power, int range, int size, SkillTargetArea targetArea, bool rooted) : base(name, description, icon, animationObject, cooldown, tier, upgradeDescriptions, hpCost,maxUses,power, range, size, targetArea, rooted)
        {
        }
    }
}