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
    public class LivingObject
    {
        const int AGILITY_TO_DOUBLE = 0;

        public string name;
        public int level;
        public GameObject gameObject;
        public bool isAlive = true;
        public bool hovered = false;
        public Player player;
        public Stats stats;
        public int team;
        private bool attackRangeShown = false;
        public List<Debuff> Debuffs = new List<Debuff>();
        public List<Buff> buffs = new List<Buff>();
        private int hp;
        public float rotation = 180;
        public int movement;
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
                if (gameObject != null)
                {
                    if (((hp * 1.0) / stats.maxHP) < 0.4f)
                    {
                        //gameObject.GetComponent<CharacterScript>().SetInjuredIdle(true);
                    }
                    else
                    {
                      //  gameObject.GetComponent<CharacterScript>().SetInjuredIdle(false);
                    }
                }
            }
        }
        public virtual void DeathAnimation()
        {

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


    }


}
