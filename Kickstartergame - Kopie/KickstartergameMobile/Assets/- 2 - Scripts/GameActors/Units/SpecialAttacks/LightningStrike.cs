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

        public override int GetSpecialDmg(Unit user, int normalAttackDamage, Unit defender)
        {
            return defender.BattleStats.GetReceivedDamage(normalAttackDamage)+defender.BattleStats.GetReceivedDamage(normalAttackDamage/2, true);
        }

        public override int GetSpecialHit(Unit user, int normalHitRate, Unit defender)
        {
            return normalHitRate;
        }

        public override void UseSpecial(Unit user, int normalAttackDamage, Unit defender)
        {
            UISystem uiController = MainScript.instance.GetSystem<UISystem>();
            int dmg=defender.InflictDamage(normalAttackDamage, user);
            int dmg2= defender.InflictDamage(normalAttackDamage/2, user, true);
            if (user.Player.IsHumanPlayer)
            {
                uiController.attackUIController.ShowDamageText(dmg);
                GameObject.FindObjectOfType<AllySpriteController>().StartAttackAnimation();
                GameObject.FindObjectOfType<EnemySpriteController>().ShakeAnimation(10 + 1f * dmg);
                GameObject.FindObjectOfType<EnemySpriteController>().StartBlinkAnimation();
                uiController.attackUIController.ShowDamageText(dmg2, true);
                GameObject.FindObjectOfType<AllySpriteController>().StartAttackAnimation();
                GameObject.FindObjectOfType<EnemySpriteController>().ShakeAnimation(10 + 1f * dmg2);
                GameObject.FindObjectOfType<EnemySpriteController>().StartBlinkAnimation();
            }
        }
    }
}
