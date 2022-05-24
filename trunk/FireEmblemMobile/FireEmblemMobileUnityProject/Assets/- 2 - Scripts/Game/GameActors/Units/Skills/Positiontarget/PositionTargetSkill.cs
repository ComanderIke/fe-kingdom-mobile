using System;
using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    [CreateAssetMenu(menuName="GameData/Skills/PositionTarget", fileName = "PositionTargetSkill")]
    public class PositionTargetSkill : Skill
    {
        public int power;
        public int range;
        public int size;
        
        public SkillTargetArea targetArea;
        public bool rooted;
        public virtual void Activate(Unit user, Tile[,] tiles, int x, int y)
        {

            foreach (var pos in GetTargetPositions())
            {
                int xPosition = x + pos.x;
                int yPosition = y + pos.y;
                if (xPosition >= 0 && xPosition < tiles.GetLength(0) && yPosition >= 0 &&
                    yPosition < tiles.GetLength(1))
                {

                    Instantiate(AnimationObject, tiles[xPosition, yPosition].GetTransform().position, Quaternion.identity, null);
                }
            }
            
        }

        protected List<Vector2Int> GetTargetPositions()
        {
            List<Vector2Int> targetPositions = new List<Vector2Int>();
            if (size > 0)
            {
                if (targetArea == SkillTargetArea.Block)
                {
                    for (int i = 0; i < size + 1; i++)
                    {
                        for (int j = 0; j < size + 1; j++)
                        {
                            targetPositions.Add(new Vector2Int(i, -j));
                            targetPositions.Add(new Vector2Int(-i, j));
                            targetPositions.Add(new Vector2Int(i, j));
                            targetPositions.Add(new Vector2Int(-i, -j));
                        }
                    }
                }
                else
                {
                    targetPositions.Add(new Vector2Int(0, 0));
                    for (int i = 1; i < size + 1; i++)
                    {
                        if (targetArea == SkillTargetArea.Line || targetArea == SkillTargetArea.Cross ||
                            targetArea == SkillTargetArea.Star)
                        {
                            targetPositions.Add(new Vector2Int(-i, 0));
                            targetPositions.Add(new Vector2Int(i, 0));
                        }

                        if (targetArea == SkillTargetArea.NormalLine || targetArea == SkillTargetArea.Cross ||
                            targetArea == SkillTargetArea.Star)
                        {
                            targetPositions.Add(new Vector2Int(0, -i));
                            targetPositions.Add(new Vector2Int(0, i));
                        }
                    }

                    if (targetArea == SkillTargetArea.Star)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                if (i != 0 && j != 0 && (i + j) <= size)
                                {
                                    targetPositions.Add(new Vector2Int(i, -j));
                                    targetPositions.Add(new Vector2Int(-i, j));
                                    targetPositions.Add(new Vector2Int(i, j));
                                    targetPositions.Add(new Vector2Int(-i, -j));
                                }

                            }
                        }
                    }


                }
            }
            else
            {
                targetPositions.Add(new Vector2Int(0, 0));

               
            }
            return targetPositions;
        }

        public void Effect(Unit user, Vector3 target)
        {
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

       
    }
}