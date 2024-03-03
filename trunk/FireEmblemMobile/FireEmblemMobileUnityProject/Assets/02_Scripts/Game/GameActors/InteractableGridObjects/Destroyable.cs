using System;
using Game.GameActors.Factions;
using Game.GameActors.Grid;
using Game.GameActors.Units;
using Game.GameActors.Units.Components;
using Game.GameActors.Units.Interfaces;
using Game.GameActors.Units.OnGameObject;
using Game.Map;
using UnityEngine;

namespace Game.GameActors.InteractableGridObjects
{
    public interface IAttackableTarget
    {
        public int Hp { get; set; }
        public int MaxHp { get; }
        GameTransformManager GameTransformManager { get; set; }
        object Clone();
        bool IsAlive();
        void Die(Unit damageSource);
    }
    public interface IAttackableTargetThatCantFightBack:IAttackableTarget
    {
    }
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Destroyable", fileName="Destroyable")]
    public class Destroyable:ScriptableObject,IGridObject, IAttackableTargetThatCantFightBack
    {
        [SerializeField]
        private GridTerrainData terrainData;
        [SerializeField]
        private GridTerrainData terrainDataAfterDeath;

        public GridTerrainData TerrainData
        {
            get
            {
                if (IsAlive())
                    return terrainData;
                return terrainDataAfterDeath;
            }
        }
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

        public GameTransformManager GameTransformManager { get; set; }

        [SerializeField]
        private Sprite sprite;
        [SerializeField]
        private Sprite spriteAfterDeath;
        public Sprite SpriteNotDestroyed
        {
            get
            {
                return sprite;

            }
        }
        public Sprite Sprite
        {
            get
            {
                if (IsAlive())
                    return sprite;
                return spriteAfterDeath;

            }
        }

        
        public GridComponent GridComponent { get; set; }
        public Faction Faction { get; set; }
        public DestroyableController Controller { get; set; }

        public void Init()
        {
            Hp = MaxHp;
        }


        private int hp = 1;
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
            return Hp > 0;
        }

        public void Die(Unit damageSource)
        {
            Debug.Log("Die Dest");
            Controller.Die();
            Faction.RemoveDestroyable(this);
            GridComponent.Tile.GridObject = null;
            OnDeath?.Invoke(this);
           
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
        public override string ToString()
        {
            return name;// + " HP: " + Hp + "/" + MaxHp+"Level: "+experienceManager.Level+ " Exp: "+experienceManager.Exp;
        }

        public static event Action<IGridObject> OnDeath;
    }
}