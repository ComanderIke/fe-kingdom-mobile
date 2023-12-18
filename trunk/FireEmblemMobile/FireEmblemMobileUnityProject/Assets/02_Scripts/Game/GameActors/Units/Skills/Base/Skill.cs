using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _02_Scripts.Game.GUI.Utility;
using Game.GameInput;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [System.Serializable]
    public class Skill:ITargetableObject
    {
        protected int maxLevel;
        public Sprite Icon;
        public SkillType SkillType { get; set; }
        public int level;
        public string Name;
        public string Description;
        public int Tier;
        public CombatSkillMixin CombatSkillMixin;
        public List<PassiveSkillMixin> passiveMixins;
        public List<ActiveSkillMixin> activeMixins;
        public Unit owner;
        public SkillTransferData skillTransferData;
       
        public Skill(string Name, string Description, Sprite icon, int tier,int maxLevel, List<PassiveSkillMixin> passiveMixins, CombatSkillMixin combatSkillMixin, List<ActiveSkillMixin> activeMixins, SkillTransferData data)
        {
            this.Name = Name;
            this.Icon = icon;
            this.Tier = tier;
            this.Description = Description;
            this.passiveMixins = passiveMixins;
            this.activeMixins = activeMixins;
            foreach (var passive in passiveMixins)
                passive.skill = this;
            foreach (var active in activeMixins)
                active.skill = this;
            this.skillTransferData = data;
            this.CombatSkillMixin = combatSkillMixin;
       

            this.maxLevel = maxLevel;


        }

        public override bool Equals(object obj)
        {
            if (obj is Skill skill)
            {
                return skill.Name == Name;
            }
            return base.Equals(obj);
        }

        public int Level
        {
            get
            {
                return level;
            }
            set
            {
               
                this.level = value;
         
                //TODO Unbind and rebind all skills (So that multipliers etc will get updated)
                Rebind();

            }
        }

        public void Rebind()
        {
            Debug.Log("REBIND");
            if (owner != null)
            {
                foreach (var passive in passiveMixins)
                {
                    passive.UnbindFromUnit(owner, this);
                    passive.BindToUnit(owner, this);
                }

                if (CombatSkillMixin != null)
                {
                    CombatSkillMixin.UnbindFromUnit(owner, this);
                    CombatSkillMixin.BindToUnit(owner, this);
                }

                foreach (var active in activeMixins)
                {
                    active.UnbindFromUnit(owner,this);
                    active.BindToUnit(owner, this);
                }
            }
            else
            {
                Debug.Log("OWNER NULL");
            }
        }

        public ActiveSkillMixin FirstActiveMixin {
            get
            {
                if (activeMixins == null || activeMixins.Count == 0)
                    return null;
                return activeMixins.First();
            }
        }

        public string GetName()
        {
            return Name;
        }
        public string GetDescription()
        {
            return Description;
        }
        public Sprite GetIcon()
        {
            return Icon;
        }
        
        public void BindSkill(Unit unit)
        {
            Debug.Log("BIND SKILL "+Name +" TO " +unit);
            owner = unit;
            Rebind();
        }
        public void UnbindSkill(Unit unit)
        {
            foreach (var passive in passiveMixins)
            {
                passive.UnbindFromUnit(owner,this);
            }
            foreach (var active in activeMixins)
            {
                active.UnbindFromUnit(owner,this);
            }
            owner = null;
        }

        public string CurrentUpgradeText()
        {
            return "";
        }

        public string NextUpgradeText()
        {
            return "";
        }


        public virtual Skill Clone()
        {
            var newSkill = new Skill(Name, Description, Icon, Tier, maxLevel,passiveMixins,CombatSkillMixin,activeMixins, skillTransferData);
            newSkill.level = Level;
            return newSkill;
        }

        public bool Upgradable()
        {
            if (this is Curse)
                return false;
            // if (this is Blessing)
            //     return false;
            return Level < maxLevel;
        }

        public bool IsActive()
        {
            return activeMixins.Count > 0;
        }

        public void Upgrade()
        {
            Level++;
        }

        public bool IsCombat()
        {
            return CombatSkillMixin!=null;
        }

        public SerializableDictionary<BlessingBP, SynergieEffects> GetSynergies()
        {
            if (CombatSkillMixin != null)
                return CombatSkillMixin.synergies;
            if (FirstActiveMixin != null)
                return FirstActiveMixin.synergies;
            if (passiveMixins.Count != 0)
                return passiveMixins[0].synergies;
            return null;
        }
    }
}