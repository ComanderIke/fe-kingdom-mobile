﻿using System;
using System.Security.Cryptography;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.OnGameObject;
using Game.Grid;
using Game.Map;
using UnityEngine;

namespace Game.GameActors.Players
{
    public interface IAttackableTarget
    {
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        object Clone();
        bool IsAlive();
        void Die();
    }
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Destroyable", fileName="Destroyable")]
    public class Destroyable:ScriptableObject,IGridObject, IAttackableTarget
    {
        public GridTerrainData terrainData;

        
        [SerializeField]
        private int maxHp;

        public int MaxHp
        {
            get
            {
                return maxHp;
            }
            set
            {
                maxHp = value;
            }
        }

        public Sprite sprite;

        
        public GridComponent GridComponent { get; set; }
        public Faction Faction { get; set; }
        public DestroyableController Controller { get; set; }

        public void Init()
        {
            Hp = MaxHp;
        }


        private int hp = 0;
        public int Hp
        {
            get
            {
                return hp;
            }
            set
            {
                hp = value;
                HpValueChanged?.Invoke();
            }
        }

        public Action HpValueChanged;
       
        public object Clone()
        {
            var ret = new Destroyable();
            ret.MaxHp = MaxHp;
            ret.Hp = Hp;
            ret.Faction = Faction;
            ret.GridComponent = GridComponent;
            ret.Controller = Controller;
            return ret;
        }

        public bool IsAlive()
        {
            return Hp >= 0;
        }

        public void Die()
        {
            Controller.Die();
        }

        public void SetAttackTarget(bool selected)
        {
            Debug.Log("Set Object as AttackTarget TODO Visuals?");
            // if (selected)
            //     visuals.UnitEffectVisual.ShowAttackable(this);
            // else
            //     visuals.UnitEffectVisual.HideAttackable();
          
        }

        public bool IsEnemy(IGridActor selectedActor)
        {
            return selectedActor.Faction.Id != Faction.Id;
        }
    }
}