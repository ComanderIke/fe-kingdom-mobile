using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private bool jumpPush;
        [SerializeField] private bool stopOnCollision;
        [field: SerializeField] private List<SkillEffectMixin> OnCollisionEffects;
        [SerializeField] private int minRange = 0;
        [SerializeField] private bool confirmPositionClick = true;
        [SerializeField] private SkillTransferData skillTransferData;
        [SerializeField] private string Animation;

        [field: SerializeField] public SkillTargetArea TargetArea { get; set; }
        public EffectType EffectType { get; set; }

        public bool ConfirmPosition()
        {
            return confirmPositionClick;
        }

        [field: SerializeField] public List<SkillEffectMixin> SkillEffects;
        [field: SerializeField] public bool Rooted { get; set; }

        public int GetRange(int level) => range[level];
        public int GetMinRange(int level) => minRange;
        public int GetSize(int level) => size[level];
        public int GetSize() => size[skill.Level];






        public bool CanTarget(Tile t)
        {
            if (jump && !jumpPush)
                return t.GridObject == null;
            return true;
        }

        private float speedPerTile = .2f;

        public virtual void Activate(Unit user, Tile[,] tiles, int x, int y)
        {
            Debug.Log("ACTIVATE POS TARGET SKILL MIXIN");
            var targetPos = new Vector2Int(x, y);
            GridSystem gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
            Vector2 directionTmp = new Vector2(targetPos.x - user.GridComponent.GridPosition.X,
                targetPos.y - user.GridComponent.GridPosition.Y).normalized;
            Vector2Int direction = new Vector2Int((int)directionTmp.x, (int)directionTmp.y);
            var targetPositions = GetTargetPositions(skill.level, direction);
            List<Vector2Int> previousPos = new List<Vector2Int>();
            previousPos.Add(new Vector2Int(user.GridComponent.GridPosition.X, user.GridComponent.GridPosition.Y));

            var chosenPrevPos = previousPos[0];
            Vector2Int unitPos = new Vector2Int(user.GridComponent.GridPosition.X, user.GridComponent.GridPosition.Y);
            foreach (var pos in targetPositions)
            {
                int xPosition = (int)(user.GridComponent.GridPosition.X + pos.x);
                int yPosition = (int)(user.GridComponent.GridPosition.Y + pos.y);
                if (xPosition == unitPos.x && yPosition == unitPos.y)
                    continue;
                Debug.Log("Targetposition: " + pos + " " + xPosition + " " + yPosition);
                if (xPosition >= 0 && xPosition < tiles.GetLength(0) && yPosition >= 0 &&
                    yPosition < tiles.GetLength(1))
                {
                    Debug.Log("Check: " + xPosition + " " + yPosition);
                    if (stopOnCollision && !gridSystem.GridLogic.IsTileAccessible(xPosition, yPosition, user, false))
                    {
                        Debug.Log("Tile no accessible: " + xPosition + " " + yPosition);
                        foreach (SkillEffectMixin effect in OnCollisionEffects)
                        {
                            if (effect is UnitTargetSkillEffectMixin unitTargetSkillEffectMixin)
                            {
                                unitTargetSkillEffectMixin.Activate(user, user, skill.Level);
                            }
                        }

                        Debug.Log("Set Unit Position: " + previousPos + " " + xPosition + " " + yPosition);
                        while (!IsValidTargetPosition(chosenPrevPos, user, gridSystem, tiles))
                        {
                            previousPos.RemoveAt(previousPos.Count - 1);

                            if (previousPos.Count == 0)
                            {
                                chosenPrevPos = new Vector2Int(unitPos.x, unitPos.y);
                                break;
                            }

                            chosenPrevPos = previousPos[previousPos.Count - 1];
                        }

                        MoveUnit(chosenPrevPos, unitPos, user);
                        // ServiceProvider.Instance.GetSystem<GridSystem>().SetUnitPosition(user, chosenPrevPos.x, chosenPrevPos.y);
                        return;
                        break;
                    }

                    Debug.Log("ACTIVATE On " + xPosition + " " + yPosition);
                    Vector2 tiledifference = new Vector2Int(xPosition, yPosition) - unitPos;

                    skillTransferData.data = tiledifference.magnitude * speedPerTile - speedPerTile;
                    if (AnimationObject != null)
                        GameObject.Instantiate(AnimationObject, tiles[xPosition, yPosition].GetTransform().position,
                            Quaternion.identity, null);

                    foreach (SkillEffectMixin effect in SkillEffects)
                    {
                        //Debug.Log("ACTIVATE SKILLEFFECTMIXINS:")
                        if (effect is TileTargetSkillEffectMixin tileTargetSkillEffectMixin)
                            tileTargetSkillEffectMixin.Activate(tiles[xPosition, yPosition], skill.Level);
                        else if (effect is UnitTargetSkillEffectMixin unitTargetSkillEffectMixin &&
                                 (Unit)tiles[xPosition, yPosition].GridObject != null)
                        {

                            unitTargetSkillEffectMixin.Activate((Unit)tiles[xPosition, yPosition].GridObject, user,
                                skill.Level);
                        }
                    }



                    previousPos.Add(new Vector2Int(xPosition, yPosition));
                    chosenPrevPos = previousPos[previousPos.Count - 1];
                }


            }

            if (jump)
            {
                int index = targetPositions.Count - 1;
                if (TargetArea == SkillTargetArea.Line)
                {
                    targetPos = new Vector2Int((int)(unitPos.x + targetPositions[index].x),
                        (int)(unitPos.y + targetPositions[index].y));
                    Debug.Log("Jump to: " + targetPos);
                }

                Debug.Log("Direction: " + direction);
                targetPos = FindValidTargetPosition(user, tiles, targetPos, gridSystem, index, unitPos,
                    targetPositions);
                MoveUnit(targetPos, unitPos, user);
                Debug.Log("Set To Position: " + targetPos);

            }


        }

        private void MoveUnit(Vector2Int targetPos, Vector2 unitPos, Unit user)
        {
            ServiceProvider.Instance.GetSystem<GridSystem>()
                .SetUnitPosition(user, targetPos.x, targetPos.y, true, false);
            Vector2 tiledifference = targetPos - unitPos;
            Debug.Log("===PosDifference: " + tiledifference + " " + tiledifference.magnitude);
            LeanTween.move(user.GameTransformManager.GameObject, new Vector2(targetPos.x, targetPos.y),
                    tiledifference.magnitude * speedPerTile)
                .setEaseOutQuad();
            if (!String.IsNullOrEmpty(Animation))
            {
                user.GameTransformManager.UnitAnimator.SetAlternateWalk(true);
                MonoUtility.DelayFunction(() => user.GameTransformManager.UnitAnimator.SetAlternateWalk(false),
                    tiledifference.magnitude * speedPerTile);
            }
        }

        bool IsValidTargetPosition(Vector2Int targetPos, Unit user, GridSystem gridSystem, Tile[,] tiles)
        {
            return (targetPos.x >= 0 && targetPos.x < tiles.GetLength(0) && targetPos.y >= 0 &&
                    targetPos.y < tiles.GetLength(1))
                   && tiles[targetPos.x, targetPos.y].HasFreeSpace() &&
                   gridSystem.GridLogic.IsTileAccessible(targetPos.x, targetPos.y, user);
        }

        private Vector2Int FindValidTargetPosition(Unit user, Tile[,] tiles, Vector2Int targetPos,
            GridSystem gridSystem,
            int index, Vector2Int unitPos, List<Vector2Int> targetPositions)
        {
            while (!IsValidTargetPosition(targetPos, user, gridSystem, tiles))
            {
                if (index >= 0)
                {
                    targetPos = new Vector2Int((int)(unitPos.x + targetPositions[index].x),
                        (int)(unitPos.y + targetPositions[index].y));
                    index--;
                }
                else
                {
                    targetPos = unitPos;
                    break;
                }
                //   Debug.Log("Checking TargetPos: "+targetPos+ " "+tiles[targetPos.x, targetPos.y].HasFreeSpace()+" "+gridSystem.GridLogic.IsTileAccessible(targetPos.x,targetPos.y,user));
            }

            return targetPos;
        }

        protected List<Vector2Int> GetTargetPositions(int level, Vector2Int direction = default)
        {
            List<Vector2Int> targetPositions = new List<Vector2Int>();
            if (direction == default)
                direction = new Vector2Int(1, 1);
            if (GetSize(level) > 0)
            {
                if (TargetArea == SkillTargetArea.Block)
                {
                    for (int i = 0; i < GetSize(level) + 1; i++)
                    {
                        for (int j = 0; j < GetSize(level) + 1; j++)
                        {
                            if (!targetPositions.Contains(new Vector2Int(i, -j)))
                                targetPositions.Add(new Vector2Int(i, -j));
                            if (!targetPositions.Contains(new Vector2Int(-i, j)))
                                targetPositions.Add(new Vector2Int(-i, j));
                            if (!targetPositions.Contains(new Vector2Int(i, j)))
                                targetPositions.Add(new Vector2Int(i, j));
                            if (!targetPositions.Contains(new Vector2Int(-i, -j)))
                                targetPositions.Add(new Vector2Int(-i, -j));
                        }
                    }
                }
                else
                {
                    //targetPositions.Add(new Vector2Int(0, 0));
                    for (int i = 1; i < GetSize(level) + 1; i++)
                    {
                        if (TargetArea == SkillTargetArea.Line && direction.x != 0 ||
                            TargetArea == SkillTargetArea.Cross ||
                            TargetArea == SkillTargetArea.Star)
                        {
                            if (!targetPositions.Contains(new Vector2Int((i * direction.x), 0)))
                                targetPositions.Add(new Vector2Int(i * direction.x, 0));
                        }

                        if (TargetArea == SkillTargetArea.Line || TargetArea == SkillTargetArea.Cross ||
                            TargetArea == SkillTargetArea.Star)
                        {
                            if (!targetPositions.Contains(new Vector2Int(0, i * direction.y)))
                                targetPositions.Add(new Vector2Int(0, i * direction.y));
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
                                    if (!targetPositions.Contains(new Vector2Int(i, -j)))
                                        targetPositions.Add(new Vector2Int(i, -j));
                                    if (!targetPositions.Contains(new Vector2Int(-i, j)))
                                        targetPositions.Add(new Vector2Int(-i, j));
                                    if (!targetPositions.Contains(new Vector2Int(i, j)))
                                        targetPositions.Add(new Vector2Int(i, j));
                                    if (!targetPositions.Contains(new Vector2Int(-i, -j)))
                                        targetPositions.Add(new Vector2Int(-i, -j));
                                }

                            }
                        }
                    }


                }
            }
            else
            {
                if (!targetPositions.Contains(new Vector2Int(0, 0)))
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


        public List<IAttackableTarget> GetAllTargets(Unit selectedUnit, Tile[,] tiles, int x, int y,
            Vector2Int direction = default)
        {
            List<IAttackableTarget> targets = new List<IAttackableTarget>();
            foreach (var pos in GetTargetPositions(skill.Level, direction))
            {
                int xPosition = x + pos.x;
                int yPosition = y + pos.y;
                if (xPosition >= 0 && xPosition < tiles.GetLength(0) && yPosition >= 0 &&
                    yPosition < tiles.GetLength(1))
                {
                    if (tiles[xPosition, yPosition].GridObject != null &&
                        tiles[xPosition, yPosition].GridObject.Faction.Id != selectedUnit.Faction.Id)
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

        public List<Vector2Int> GetCastTargets(Unit unit, Tile[,] tiles, int level, int x, int y)
        {
            var castTargets = new List<Vector2Int>();
            var castPosition = new Vector2Int(x, y);
            var gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
            if (GetRange(level) > 0)
            {
                for (int i = 0; i <= GetRange(level); i++)
                {
                    for (int j = 0; j <= GetRange(level); j++)
                    {
                        if (i == 0 && j == 0)
                            continue;
                        if ((i + j) <= GetRange(level))
                        {
                            var pos = castPosition + new Vector2Int(i, j);
                            if(!gridSystem.IsOutOfBounds(pos.x,pos.y))
                                castTargets.Add(pos);
                            pos = castPosition + new Vector2Int(-i, j);
                            if(!gridSystem.IsOutOfBounds(pos.x,pos.y))
                                castTargets.Add(pos);
                            pos = castPosition + new Vector2Int(i, -j);
                            if(!gridSystem.IsOutOfBounds(pos.x,pos.y))
                                castTargets.Add(pos);
                            pos = castPosition + new Vector2Int(-i, -j);
                            if(!gridSystem.IsOutOfBounds(pos.x,pos.y))
                                castTargets.Add(pos);

                        }

                    }
                }
            }

            return castTargets;

        }

        public DamageSkillEffectMixin GetDamageMixin()
        {
           return (DamageSkillEffectMixin)SkillEffects.First(s => s is DamageSkillEffectMixin);
        }
    }
}