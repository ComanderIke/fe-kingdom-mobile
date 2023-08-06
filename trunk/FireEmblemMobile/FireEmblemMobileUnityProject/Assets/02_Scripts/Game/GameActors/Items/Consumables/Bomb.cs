using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Skills;
using Game.Grid;
using Game.Mechanics;
using UnityEngine;

namespace _02_Scripts.Game.GameActors.Items.Consumables
{
    interface IPosTargeted
    {
        bool Rooted { get; set; }
        void Activate(Unit selectedUnit, Tile[,] gridSystemTiles, int p2, int p3);
        List<IAttackableTarget> GetAllTargets(Unit selectedUnit, Tile[,] gridSystemTiles, int i, int i1);
        int GetSize();
        SkillTargetArea TargetArea { get; set; }
        EffectType EffectType { get; set; }
        bool ConfirmPosition();
    }
    interface IThrowableItem
    {
        public int Range { get; set; }
        public EffectType EffectType { get; set; } 
    }

    public enum EffectType
    {
        Heal,
        Good,
        Bad,
        Neutral
    }
    public class Bomb : ConsumableItem, IThrowableItem, IEquipableCombatItem
    {
        public Debuff appliedDebuff;
        public int power;
        public int Range { get; set; }
        public EffectType EffectType { get; set; }
        public bool Rooted { get; set; }
        public void Activate(Unit selectedUnit, Tile[,] gridSystemTiles, int x, int y)
        {
            var targets = GetAllTargets(selectedUnit, gridSystemTiles, x, y);
            foreach (var skillTarget in targets)
            {
                Debug.Log("target"+skillTarget);
                if (skillTarget is Unit targetUnit)
                {
                    Debug.Log("TARGET "+targetUnit.Name);
                    targetUnit.InflictFixedDamage(selectedUnit, power, DamageType.Physical);
                }
            }
            Debug.Log("ACTIVATE BOMB");
        }

        void AddToPositionList(List<Vector2> positions, Vector2 pos)
        {
            if(!positions.Contains(pos))
                positions.Add(pos);
        }
        public List<Vector2> GetTargetPositions( Tile[,] gridSystemTiles, Vector2 cursorPosition)
        {
            var targetPositions = new List<Vector2>();
            if (Size > 0)
            {
                if (TargetArea == SkillTargetArea.Block)
                {
                    for (int i = 0; i < Size + 1; i++)
                    {
                        for (int j = 0; j < Size + 1; j++)
                        {
                            AddToPositionList(targetPositions,new Vector2(i, -j) + cursorPosition);
                            AddToPositionList(targetPositions,new Vector2(i, j) + cursorPosition);
                            AddToPositionList(targetPositions,new Vector2(-i, -j) + cursorPosition);
                            AddToPositionList(targetPositions,new Vector2(-i, j) + cursorPosition);
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < Size + 1; i++)
                    {
                        if (TargetArea == SkillTargetArea.Line || TargetArea == SkillTargetArea.Cross ||
                            TargetArea == SkillTargetArea.Star)
                        {
                            AddToPositionList(targetPositions,new Vector2(i, 0) + cursorPosition);
                            AddToPositionList(targetPositions,new Vector2(-i, 0) + cursorPosition);

                        }

                        if (TargetArea == SkillTargetArea.NormalLine || TargetArea == SkillTargetArea.Cross ||
                            TargetArea == SkillTargetArea.Star)
                        {
                            AddToPositionList(targetPositions,new Vector2(0, -i) + cursorPosition);
                            AddToPositionList(targetPositions,new Vector2(0, i) + cursorPosition);

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
                                    AddToPositionList(targetPositions,new Vector2(i, -j) + cursorPosition);
                                    AddToPositionList(targetPositions,new Vector2(i, j) + cursorPosition);
                                    AddToPositionList(targetPositions,new Vector2(-i, -j) + cursorPosition);
                                    AddToPositionList(targetPositions,new Vector2(-i, j) + cursorPosition);
                                }

                            }
                        }
                    }
                }
            }

            for (int i = targetPositions.Count - 1; i > 0; i--)
            {
                if (!CheckTile(gridSystemTiles.GetLength(0), gridSystemTiles.GetLength(1), targetPositions[i]))
                {
                    targetPositions.Remove(targetPositions[i]);
                }
            }

            
            return targetPositions;
        }

        private bool CheckTile (int width, int height, Vector2 position)
        {
            return position.x > 0 && position.y > 0 && position.x < width && position.y < height;
        }

        public List<IAttackableTarget> GetAllTargets(Unit selectedUnit, Tile[,] gridSystemTiles, int cursorX, int cursorY)
        {
            var targets = new List<IAttackableTarget>();
            foreach (var pos in GetTargetPositions(gridSystemTiles, new Vector2(cursorX,cursorY)))
            {
                int x = (int)pos.x;
                int y = (int)pos.y;
                Debug.Log("targetposition"+pos);
                if (gridSystemTiles[x,y].GridObject!=null)
                {
                   
                    if (gridSystemTiles[x, y].GridObject is Unit targetUnit)
                    {
                        Debug.Log("Unit"+targetUnit );
                        if(selectedUnit.Faction.IsOpponentFaction(targetUnit.Faction))
                            targets.Add(targetUnit);
                    }
                       
                }
            }
            return targets;
        }

        public int Size { get; set; }
        
        public SkillTargetArea TargetArea{ get; set; }
        public Bomb(int power, int range, int size, SkillTargetArea targetArea, EffectType effectType,Debuff appliedDebuff, string name, string description, int cost, int rarity,int maxStack,Sprite sprite) : base(name, description, cost,rarity, maxStack,sprite, ItemTarget.Position)
        {
            this.power = power;
            this.Range = range;
            this.Size = size;
            this.TargetArea = targetArea;
            this.EffectType = effectType;
            this.appliedDebuff = appliedDebuff;
        }
    }
}