using System;
using System.Collections.Generic;
using System.Linq;
using LostGrace;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

namespace Game.GameActors.Units.CharStateEffects
{
    public abstract class BuffDebuffBaseData : ScriptableObject
    {
        [SerializeField] public GameObject vfx;
        [field:SerializeField] public Sprite Icon { get; set; }

        public virtual void Apply(Unit caster, Unit target, int skilllevel)
        {
            if (vfx != null)
            {
                var go = Instantiate(vfx, null);
                go.transform.position = target.GameTransformManager.GetCenterPosition();
            }
        }

        public virtual void TakeEffect(Unit unit)
        {
        }

        public abstract IEnumerable<EffectDescription> GetEffectDescription();
    }
    public enum BuffType
    {
        CurseResistance,
        MagicResistance,
        Cleansing,
        Regeneration,
        Stealth,
        AbsorbingDmg,
        Invulnerability,
        Stride,
        Rage
        //ClearMovement
        //Add as needed
    }

    [CreateAssetMenu(menuName = "GameData/Buffs/BuffData")]
    public class BuffData : BuffDebuffBaseData
    {
        [SerializeField] private BuffType buffType;

        public override void Apply(Unit caster, Unit target, int skilllevel)
        {
            base.Apply(caster,target,skilllevel);
            switch (buffType)
            {
                case BuffType.Cleansing:
                    //TODO
                    break;
                
            }
        }
        public override void TakeEffect(Unit unit)
        {
            switch (buffType)
            {
                case BuffType.Cleansing: //TODO
                                         break;
                
            }
        }
        public override IEnumerable<EffectDescription> GetEffectDescription()
        {
            return new List<EffectDescription>(){new("Grants", buffType.ToString(), buffType.ToString())};
        }
    }

    [Serializable]
    public enum DebuffType
    {
        Stunned,
        Silenced,
        Poisened,
        Enwebed,
        Slept,
        Tempted,
        Frog
    }
    [CreateAssetMenu(menuName = "GameData/Buffs/DebuffData")]
    public class DebuffData : BuffDebuffBaseData
    {
        [SerializeField] public DebuffType debuffType;
        [SerializeField] public List<DebuffData> negateTypes;

        public override void Apply(Unit caster, Unit target, int skilllevel)
        {
            base.Apply(caster,target,skilllevel);
            foreach (var negate in negateTypes)
            {
                var tmpList = target.StatusEffectManager.Buffs.Where(d => d == negate);
                foreach (var entry in tmpList)
                {
                    target.StatusEffectManager.Buffs.Remove(entry);
                }
            }
            switch (debuffType)
            {
                case DebuffType.Tempted:
                    target.Faction.RemoveUnit(target);
                    caster.Faction.AddUnit(target);
                    break;
                
            }
        }
        public override IEnumerable<EffectDescription> GetEffectDescription()
        {
            return new List<EffectDescription>(){new("Applies: ", debuffType.ToString(), debuffType.ToString())};
        }
        public override void TakeEffect(Unit unit)
        {
            switch (debuffType)
            {
                case DebuffType.Stunned: unit.TurnStateManager.UnitTurnFinished(); //unit set phys invulnerable
                    break;
                
            }
        }
    }
    public abstract class BuffDebuffBase : ScriptableObject
    {

        public BuffDebuffBaseData BuffData;
        [SerializeField] public int[] duration;
       
        protected int level;
        public List<EffectDescription> GetEffectDescription(int level)
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("For "+duration[level]+" Turns: ", "", ""));
            list.AddRange(BuffData.GetEffectDescription());
           
            return list;
        }
        public virtual void Apply(Unit caster, Unit target, int skilllevel)
        {
            BuffData.Apply(caster,target,skilllevel);
          

            Debug.Log("SPAWNED VFX");
            this.level = skilllevel;
        }
        public void IncreaseCurrentDuration()
        {
            duration[level]++;
        }
        public virtual bool TakeEffect(Unit unit)
        {
            BuffData.TakeEffect(unit);
           
            Debug.Log("TAKE EFFECT"+duration[level]);
            duration[level]--;
            return duration[level] <= 0;
        }

        public virtual void Unapply(Unit target)
        {
            level = 0;
        }

        public int GetDuration()
        {
            return duration[level];
        }
    }
}