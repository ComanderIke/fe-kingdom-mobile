using AssemblyCSharp;
using Assets.Scripts.Characters.Attributes;
using Assets.Scripts.Characters.Debuffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    public enum StatAttribute
    {
        Attack,
        Defense,
        Spirit,
        Speed,
        Accuracy,
        HP
    }
    public abstract class LivingObject
    {
        const int AGILITY_TO_DOUBLE = 0;

        public string name;
        public int level;
        public GameObject gameObject;
        public bool isAlive = true;
        public bool hovered = false;
        public Player player;
        public Stats stats;
        public bool isWaiting;
        public int team;
        public int movRange = 4;
        public int x;
        public int y;
        public Vector2 OldPosition;
        private bool attackRangeShown = false;
        public List<Debuff> Debuffs = new List<Debuff>();
        public List<Buff> buffs = new List<Buff>();
        public List<int> AttackRanges;
        private int hp;
        public float rotation = 180;
        public bool hasMoved = false;
        public bool hasAttacked = false;
        public Sprite activeSpriteObject;
        public int movement;
        private bool selected = false;
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
            }
        }
        private HPBarOnMap hpBar;
        public int HP
        {
            get { return hp; }
            set
            {
                if (value > stats.maxHP)
                    hp = stats.maxHP;
                else
                {
                    hp = value;
                }
                if (hp <= 0)
                {
                    if (isAlive)
                    {
                        isAlive = false;
                        DeathAnimation();
                    }
                    hp = 0;
                }
                if(hpBar!=null)
                    hpBar.SetCurrentHealth(hp);
            }
        }

        public LivingObject()
        {
           
        }
        public void Init()
        {
            hpBar = gameObject.GetComponentInChildren<HPBarOnMap>();
            hpBar.SetMaxHealth(stats.maxHP);
            hpBar.SetCurrentHealth(hp);
        }
        public virtual void DeathAnimation()
        {

        }
        public virtual void SetPosition(int x, int y)
        {
            OldPosition = new Vector2(this.x, this.y);
            MainScript m = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>();
            m.gridScript.fields[(int)GetPositionOnGrid().x, (int)GetPositionOnGrid().y].character = null;
            this.x = x;
            this.y = y;
            gameObject.transform.localPosition = new Vector3(x, y, 0);
            m.gridScript.fields[x, y].character = this;
        }

        public void SetRotation(int degrees)
        {
            rotation = degrees;
            gameObject.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.up);
        }
        public bool IsAttackRangeShown()
        {
            return attackRangeShown;
        }
        public void SetAttackRangeShown(bool v)
        {
            attackRangeShown = v;
        }
        public  bool CanDoubleAttack(LivingObject c)
        {
            if (stats.speed > c.stats.speed + AGILITY_TO_DOUBLE)
            {
                return true;
            }
            return false;
        }
        public virtual int getCrit()
        {
            return stats.accuracy;
        }
        public virtual int GetHitAgainstTarget(LivingObject target)
        {
            return (int)Mathf.Clamp(stats.accuracy - target.stats.speed, 0, 100);
        }
        public virtual int GetHitRate()
        {
            return stats.accuracy;
        }
        public Vector2 GetPositionOnGrid()
        {
            return new Vector2(this.x, this.y);
        }
        public void SetInternPosition(int x, int y)
        {
            MainScript m = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>();
            m.gridScript.fields[(int)GetPositionOnGrid().x, (int)GetPositionOnGrid().y].character = null;
            m.gridScript.fields[x, y].character = (Character)this;
            this.x = x;
            this.y = y;
        }
        public bool CanAttack(int range)
        {
            return AttackRanges.Contains(range);
        }
        public bool CanKillTarget(LivingObject target)
        {
            return GetDamageAgainstTarget(target) >= target.HP;
        }
       
        public virtual int GetDamageAgainstTarget(LivingObject target)
        {
            return 0;
        }
        public virtual int GetTotalDamageAgainstTarget(LivingObject target)
        {
            return 0;

        }

    }


}
