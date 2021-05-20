using System.Collections.Generic;
using System.Linq;
using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.GUI;
using Game.GUI.PopUpText;
using Game.Manager;
using Game.States;
using GameEngine;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Utility;

namespace Game.Mechanics
{
    public class PopUpTextSystem : IEngineSystem, IDependecyInjection
    {
        public ILevelUpRenderer levelUpRenderer;//injected
        public IExpRenderer ExpRenderer;
        private List<Faction> Factions;

        public void Init()
        {


        }

        private void UnitDamaged(Unit unit, int dmg, bool magic, bool crit, bool eff)
        {
            if (unit.GameTransformManager.GameObject == null) //Only if Real instantiaed Unit takes dmg(not cloned battlepreview unit)
                return;
           
            float scale = 1.0f;
            Color color = ColorManager.Instance.PhysicalDmgColor;
            if (magic) //different color;
                color = ColorManager.Instance.MagicDmgColor;

            if (crit) // Bigger and Brighter?
            {
          
                scale = 1.20f;
                if (!magic)
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

            DamagePopUp.Create(unit.GameTransformManager.GetCenterPosition(), dmg, color,scale);
            
        }
        public void Deactivate()
        {
            Unit.OnUnitDamaged -= UnitDamaged;
        }

        public void Activate()
        {
            Unit.OnUnitDamaged += UnitDamaged;
        }

  

    }
}