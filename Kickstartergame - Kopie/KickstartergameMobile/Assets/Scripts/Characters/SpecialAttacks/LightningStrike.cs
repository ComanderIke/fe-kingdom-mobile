using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Characters.SpecialAttacks
{
    public class LightningStrike : SpecialAttack
    {
        public LightningStrike():base("LightningStrike", "", 2)
        {

        }

        public override int GetSpecialDmg(LivingObject user, int normalAttackDamage, LivingObject defender)
        {
            return defender.BattleStats.GetReceivedDamage(normalAttackDamage)+defender.BattleStats.GetReceivedDamage(normalAttackDamage/2, true);
        }

        public override int GetSpecialHit(LivingObject user, int normalHitRate, LivingObject defender)
        {
            return normalHitRate;
        }

        public override void UseSpecial(LivingObject user, int normalAttackDamage, LivingObject defender)
        {
            UIController uiController = MainScript.GetInstance().GetController<UIController>();
            int dmg=defender.InflictDamage(normalAttackDamage, user);
            int dmg2= defender.InflictDamage(normalAttackDamage/2, user, true);
            if (user.Player.IsHumanPlayer)
            {
                uiController.attackUIController.ShowDamageText(dmg);
                uiController.attackUIController.ShowDamageText(dmg2, true);
            }
        }
    }
}
