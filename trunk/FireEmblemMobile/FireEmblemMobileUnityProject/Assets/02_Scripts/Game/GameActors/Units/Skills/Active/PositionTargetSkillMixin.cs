using System;
using System.Collections;
using System.Collections.Generic;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.GameActors.Players;
using Game.GameActors.Units.Numbers;
using Game.GameInput;
using Game.Grid;
using Game.Manager;
using Game.Map;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    public enum PositionTargetDamageType
    {
        PhysDamage,
        MagDamage,
        Heal,
        None
    }
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Active/PositionTarget", fileName = "PositionTargetSkillMixin")]
        public class PositionTargetSkillMixin : ActiveSkillMixin, IPosTargeted
        {
            
    
        [field: SerializeField] private int[] range;
        [field: SerializeField] private int[] size;
        [SerializeField] private bool jump;
        [SerializeField] private int minRange = 0;
        [SerializeField] private bool confirmPositionClick = true;
       
        [field:SerializeField]public SkillTargetArea TargetArea { get; set; }
        public EffectType EffectType { get; set; }
        public bool ConfirmPosition()
        {
            return confirmPositionClick;
        }

        [field:SerializeField]public List<SkillEffectMixin> SkillEffects;
        [field:SerializeField]public bool Rooted { get; set; }
        
        public int GetRange(int level)  => range[level];
        public int GetMinRange(int level)  => minRange;
        public int GetSize(int level)  => size[level];
        public int GetSize()  => size[skill.Level];


       

        

        public bool CanTarget(Tile t)
        {
            if (jump)
                return t.GridObject == null;
            return true;
        }
        public virtual void Activate(Unit user, Tile[,] tiles, int x, int y)
        {
Debug.Log("ACTIVATE POS TARGET SKILL MIXIN");
            foreach (var pos in GetTargetPositions(skill.Level))
            {
                int xPosition = x + pos.x;
                int yPosition = y + pos.y;
                if (xPosition >= 0 && xPosition < tiles.GetLength(0) && yPosition >= 0 &&
                    yPosition < tiles.GetLength(1))
                {
                    Debug.Log("ACTIVATE On "+xPosition+" "+yPosition);
                    if(AnimationObject!=null)
                        GameObject.Instantiate(AnimationObject, tiles[xPosition, yPosition].GetTransform().position, Quaternion.identity, null);
                    foreach (SkillEffectMixin effect in SkillEffects)
                    {
                        if(effect is TileTargetSkillEffectMixin tileTargetSkillEffectMixin)
                            tileTargetSkillEffectMixin.Activate(tiles[x,y], skill.Level);
                        else if (effect is UnitTargetSkillEffectMixin unitTargetSkillEffectMixin&&(Unit)tiles[x,y].GridObject!=null)
                        {
                            unitTargetSkillEffectMixin.Activate((Unit)tiles[x,y].GridObject, user,skill.Level);
                        }
                    }
                }
            }
            
            if(jump)
                    ServiceProvider.Instance.GetSystem<GridSystem>().SetUnitPosition(user, x, y);
            
            
        }

        protected List<Vector2Int> GetTargetPositions(int level)
        {
            List<Vector2Int> targetPositions = new List<Vector2Int>();
            if (GetSize(level) > 0)
            {
                if (TargetArea == SkillTargetArea.Block)
                {
                    for (int i = 0; i < GetSize(level) + 1; i++)
                    {
                        for (int j = 0; j < GetSize(level) + 1; j++)
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
                    for (int i = 1; i < GetSize(level) + 1; i++)
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
                        for (int i = 0; i < GetSize(level); i++)
                        {
                            for (int j = 0; j < GetSize(level); j++)
                            {
                                if (i != 0 && j != 0 && (i + j) <= GetSize(level))
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
        

        public int GetCastRangeIncrease(Attributes statsAttributes)
        {
            return statsAttributes.INT / 5;
        }


        public List<IAttackableTarget> GetAllTargets(Unit selectedUnit, Tile[,] tiles, int x, int y)
        {
            List<IAttackableTarget> targets = new List<IAttackableTarget>();
            foreach (var pos in GetTargetPositions(skill.Level))
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

        public List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();
            foreach (var skillEffect in SkillEffects)
            {
                list.AddRange(skillEffect.GetEffectDescription(level));
            }
            return list;
        }
        }
}