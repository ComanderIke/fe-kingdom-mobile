using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Skills;
using Game.Grid;
using UnityEngine;

namespace _02_Scripts.Game.GameActors.Items.Consumables
{
    interface IPosTargeted
    {
        bool Rooted { get; set; }
        void Activate(Unit selectedUnit, Tile[,] gridSystemTiles, int p2, int p3);
        List<IAttackableTarget> GetAllTargets(Unit selectedUnit, Tile[,] gridSystemTiles, int i, int i1);
        int Size { get; set; }
        SkillTargetArea TargetArea { get; set; }
    }
    interface IThrowableItem:IPosTargeted
    {
        public int Range { get; set; }
    } 
    public class Bomb : ConsumableItem, IThrowableItem
    {
        public Debuff appliedDebuff;
        public int power;
        public int Range { get; set; }
        public bool Rooted { get; set; }
        public void Activate(Unit selectedUnit, Tile[,] gridSystemTiles, int p2, int p3)
        {
            throw new System.NotImplementedException();
        }

        public List<IAttackableTarget> GetAllTargets(Unit selectedUnit, Tile[,] gridSystemTiles, int i, int i1)
        {
            throw new System.NotImplementedException();
        }

        public int Size { get; set; }
        
        public SkillTargetArea TargetArea{ get; set; }
        public Bomb(int power, int range, int size, SkillTargetArea targetArea, Debuff appliedDebuff, string name, string description, int cost, int rarity,int maxStack,Sprite sprite) : base(name, description, cost,rarity, maxStack,sprite, ItemTarget.Position)
        {
            this.power = power;
            this.Range = range;
            this.Size = size;
            this.TargetArea = targetArea;
            this.appliedDebuff = appliedDebuff;
        }
    }
}