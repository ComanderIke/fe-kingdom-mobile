using System.Collections.Generic;
using Game.GameActors.Factions;
using Game.GameActors.Units;
using Game.GUI.PopUpText;
using Game.GUI.Renderer;
using Game.Utility;
using GameEngine;
using UnityEngine;

namespace Game.Systems
{
    public class PopUpTextSystem : IEngineSystem
    {
        public ILevelUpRenderer levelUpRenderer;//injected
        private List<Faction> Factions;

        public void Init()
        {


        }

        private void UnitDamaged(Unit unit, int dmg, DamageType damageType, bool crit, bool eff)
        {
            if (unit.GameTransformManager == null)
                return;
            if (unit.GameTransformManager.GameObject == null) //Only if Real instantiaed Unit takes dmg(not cloned battlepreview unit)
                return;
           
            float scale = 1.0f;
            Color color = ColorManager.Instance.PhysicalDmgColor;
            if (damageType == DamageType.Magic) //different color;
                color = ColorManager.Instance.MagicDmgColor;
            if (damageType == DamageType.Faith) //different color;
                color = ColorManager.Instance.MainWhiteColor;

            if (crit) // Bigger and Brighter?
            {
          
                scale = 1.20f;
                if (damageType != DamageType.Magic && damageType != DamageType.Faith)
                {
                    color.r += 0.4f;
                    color.g -= 0.1f;
                     color.b -= 0.1f;
                }
                else
                {
                    color.r -= 0.1f;
                    color.g -= 0.1f;
                    color.b += 0.4f;
                }
                // color.g += 0.05f;
                // color.b += 0.05f;
            }

            if (eff) // Much Bigger and different Color
            {
                scale = 1.5f;
                color = ColorManager.Instance.EffectiveDmgColor;
            }

            if (dmg == 0)
                color = ColorManager.Instance.MainWhiteColor;
            if (dmg < 0)
                color = ColorManager.Instance.MainGreenColor;

            Debug.Log("Unit Damaged: "+unit+" "+unit.GameTransformManager.GetCenterPosition());
            DamagePopUp.Create(unit.GameTransformManager.GetCenterPosition(), dmg, TextStyle.Damage,scale);
            
        }

        void UnitHealed(Unit unit, int healAmount)
        {
            if (unit.GameTransformManager.GameObject == null) //Only if Real instantiaed Unit takes dmg(not cloned battlepreview unit)
                return;
           
            float scale = 1.0f;
            Color color = ColorManager.Instance.MainGreenColor;
            Debug.Log("Unit healed: "+unit);
            DamagePopUp.Create(unit.GameTransformManager.GetCenterPosition(), healAmount, TextStyle.Heal,scale);
        }
        public void Deactivate()
        {
            Unit.OnUnitDamaged -= UnitDamaged;
            Unit.OnUnitHealed -= UnitHealed;
        }

        public void Activate()
        {
            Unit.OnUnitDamaged -= UnitDamaged;
            Unit.OnUnitHealed -= UnitHealed;
            Unit.OnUnitDamaged += UnitDamaged;
            Unit.OnUnitHealed += UnitHealed;
        }

  

    }
}