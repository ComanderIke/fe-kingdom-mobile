using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Mechanics;
using UnityEditor.UIElements;
using UnityEngine;

namespace Game.GameResources
{
    [Serializable]
    public class StatusEffectData
    {
        public Sprite statBuffIcon;
        public Sprite statDebuffIcon;
        public Sprite statBuffAndDebuffIcon;
        public Color debuffColor;
        public Color buffColor;
        private StatusEffectBP[] allStatusEffectBps;

        public void OnValidate()
        {
            allStatusEffectBps = GameBPData.GetAllInstances<StatusEffectBP>();
        }
    }

    public enum DurationType
    {
        Turns,
        Battles
    }
    [SerializeField]
  
    public abstract class StatusEffectBP:ScriptableObject
    {
        public Sprite Icon;
        public bool Positive;
        public string Label;
        public string Description;
        public int duration;
        public DurationType DurationType;
        public List<StatusEffectBP> negatedEffects;


    }
    public abstract class StatusEffect
    {
        public Unit affectedUnit;
        public Sprite Icon;
        public bool Positive;
        public string Label;
        public string Description;
        public int duration;
        public List<StatusEffect> negatedEffects;
        public DurationType DurationType;
        public abstract void OnStartTurn();
        public abstract void OnApplied();
        public abstract void OnRemoved();

    }
    [CreateAssetMenu(menuName = "GameData/StatusEffect/Stunned", fileName = "StatusEffect1")]
    public class StunnedBP:StatusEffectBP
    {

    }
    public class Stunned:StatusEffect
    {
        public override void OnStartTurn()
        {
            //affectedUnit.Wait();
        }

        public override void OnApplied()
        {
            throw new NotImplementedException();
        }

        public override void OnRemoved()
        {
            throw new NotImplementedException();
        }
    }
    public class Poisened:StatusEffect
    {
        public override void OnStartTurn()
        {
            //affectedUnit.TakeFixedDamage();
        }

        public override void OnApplied()
        {
            throw new NotImplementedException();
        }

        public override void OnRemoved()
        {
            throw new NotImplementedException();
        }
    }
    public class Blinded:StatusEffect
    {
        public override void OnStartTurn()
        {
            
        }

        public override void OnApplied()
        {
            //affectedUnit.BonusStats.AddHit(-30);
        }

        public override void OnRemoved()
        {
            //affectedUnit.BonusStats.AddHit(30);
        }
    }
    public class Silenced:StatusEffect
    {
        public override void OnStartTurn()
        {
            
        }

        public override void OnApplied()
        {
            //affectedUnit.SetSilenced(true);
        }

        public override void OnRemoved()
        {
            //affectedUnit.SetSilenced(false);
        }
    }
    public class Immobilized:StatusEffect
    {
        public override void OnStartTurn()
        {
            
        }

        public override void OnApplied()
        {
            //affectedUnit.SetMove(0);
        }

        public override void OnRemoved()
        {
            //affectedUnit.SetMove(default)
        }
    }
    public class Shielded:StatusEffect
    {
        public override void OnStartTurn()
        {
            
        }

        public override void OnApplied()
        {
            //affectedUnit.OnBeforeTakeDamage+=OnTakeDamage
        }

        public void OnBeforeTakeDamage(ref AttackData attackData)//directly reduce dmg variable
        {
            attackData.Dmg /= 2;
            //Reduce Status Effect Battle Duration RemoveStatusEffect();
        }

        public override void OnRemoved()
        {
            //affectedUnit.SetMove(default)
        }
    }
}