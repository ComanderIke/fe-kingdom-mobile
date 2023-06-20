using System;
using System.Collections.Generic;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.GameActors.Players;
using Game.GameActors.Units.Numbers;
using Game.GameInput;
using Game.Grid;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [System.Serializable]
        public class PositionTargetSkill : ActivatedSkill, IPosTargeted
    {
        public int power;
        public int Range { get; set; }
        public int Size { get; set; }
       

        public SkillTargetArea TargetArea { get; set; }
        public bool Rooted { get; set; }

        public PositionTargetSkill(string name, string description, Sprite icon, GameObject animationObject, int cooldown, int tier,string[] upgradeDescriptions,int hpCost, int maxUses, int power, int range, int size, SkillTargetArea targetArea, bool rooted) : base(name,description, icon, animationObject, cooldown,tier, upgradeDescriptions, hpCost,maxUses)
        {
            this.Rooted = rooted;
            this.TargetArea = targetArea;
            this.Size = size;
            this.Range = range;
            this.power = power;
        }

        public virtual void Activate(Unit user, Tile[,] tiles, int x, int y)
        {

            foreach (var pos in GetTargetPositions())
            {
                int xPosition = x + pos.x;
                int yPosition = y + pos.y;
                if (xPosition >= 0 && xPosition < tiles.GetLength(0) && yPosition >= 0 &&
                    yPosition < tiles.GetLength(1))
                {

                    GameObject.Instantiate(AnimationObject, tiles[xPosition, yPosition].GetTransform().position, Quaternion.identity, null);
                }
            }
            
        }

        protected List<Vector2Int> GetTargetPositions()
        {
            List<Vector2Int> targetPositions = new List<Vector2Int>();
            if (Size > 0)
            {
                if (TargetArea == SkillTargetArea.Block)
                {
                    for (int i = 0; i < Size + 1; i++)
                    {
                        for (int j = 0; j < Size + 1; j++)
                        {
                            if(!targetPositions.Contains(new Vector2Int(i, -j)))
                                targetPositions.Add(new Vector2Int(i, -j));
                            if(!targetPositions.Contains(new Vector2Int(-i, j)))
                                targetPositions.Add(new Vector2Int(-i, j));
                            if(!targetPositions.Contains(new Vector2Int(i, j)))
                                targetPositions.Add(new Vector2Int(i, j));
                            if(!targetPositions.Contains(new Vector2Int(-i, -j)))
                                targetPositions.Add(new Vector2Int(-i, -j));
                        }
                    }
                }
                else
                {
                    targetPositions.Add(new Vector2Int(0, 0));
                    for (int i = 1; i < Size + 1; i++)
                    {
                        if (TargetArea == SkillTargetArea.Line || TargetArea == SkillTargetArea.Cross ||
                            TargetArea == SkillTargetArea.Star)
                        {
                            if(!targetPositions.Contains(new Vector2Int(-i, 0)))
                             targetPositions.Add(new Vector2Int(-i, 0));
                            if(!targetPositions.Contains(new Vector2Int(i, 0)))
                                targetPositions.Add(new Vector2Int(i, 0));
                        }

                        if (TargetArea == SkillTargetArea.NormalLine ||TargetArea == SkillTargetArea.Cross ||
                            TargetArea == SkillTargetArea.Star)
                        {
                            if(!targetPositions.Contains(new Vector2Int(0, -i)))
                                targetPositions.Add(new Vector2Int(0, -i));
                            if(!targetPositions.Contains(new Vector2Int(0, i)))
                                targetPositions.Add(new Vector2Int(0, i));
                        }
                    }

                    if (TargetArea == SkillTargetArea.Star)
                    {
                        for (int i = 0; i < Size; i++)
                        {
                            for (int j = 0; j < Size; j++)
                            {
                                if (i != 0 && j != 0 && (i + j) <= Size)
                                {
                                    if(!targetPositions.Contains(new Vector2Int(i, -j)))
                                        targetPositions.Add(new Vector2Int(i, -j));
                                    if(!targetPositions.Contains(new Vector2Int(-i, j)))
                                        targetPositions.Add(new Vector2Int(-i, j));
                                    if(!targetPositions.Contains(new Vector2Int(i, j)))
                                        targetPositions.Add(new Vector2Int(i, j));
                                    if(!targetPositions.Contains(new Vector2Int(-i, -j)))
                                        targetPositions.Add(new Vector2Int(-i, -j));
                                }

                            }
                        }
                    }


                }
            }
            else
            {
                if(!targetPositions.Contains(new Vector2Int(0, 0)))
                    targetPositions.Add(new Vector2Int(0, 0));

               
            }
            return targetPositions;
        }

        public void Effect(Unit user, Vector3 target)
        {
        }

        public override List<EffectDescription> GetEffectDescription()
        {
            return null;
        }

        public override bool CanTargetCharacters()
        {
            throw new NotImplementedException();
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            throw new NotImplementedException();
        }

        public int GetCastRangeIncrease(Attributes statsAttributes)
        {
            return statsAttributes.INT / 5;
        }


        public List<IAttackableTarget> GetAllTargets(Unit selectedUnit, Tile[,] tiles, int x, int y)
        {
            List<IAttackableTarget> targets = new List<IAttackableTarget>();
            foreach (var pos in GetTargetPositions())
            {
                int xPosition = x + pos.x;
                int yPosition = y + pos.y;
                if (xPosition >= 0 && xPosition < tiles.GetLength(0) && yPosition >= 0 &&
                    yPosition < tiles.GetLength(1))
                {
                    if (tiles[xPosition, yPosition].GridObject!=null&&tiles[xPosition, yPosition].GridObject.Faction.Id!=selectedUnit.Faction.Id)
                    {
                        targets.Add((IAttackableTarget)tiles[xPosition, yPosition].GridObject);
                    }
                }
            }

            return targets;
        }
    }
}