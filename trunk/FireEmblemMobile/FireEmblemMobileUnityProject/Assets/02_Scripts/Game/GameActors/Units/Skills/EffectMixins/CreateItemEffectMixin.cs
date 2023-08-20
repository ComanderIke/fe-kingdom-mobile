using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GameResources;
using Game.Manager;
using Game.Map;
using Game.WorldMapStuff.Model;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/CreateItemEffect", fileName = "CreateItemEffect")]
    public class CreateItemEffectMixin : SelfTargetSkillEffectMixin
    {
        public ItemBP itemToCreate;
        public bool equipIfPossible = false;
        public bool randomPotion = false;
       
        public override void Activate(Unit target, int level)
        {
            // check if objects in the way
            if (randomPotion)
            {
                var potion = GameBPData.Instance.GetRandomPotion();
                if (target.CombatItem1 == null&&potion is IEquipableCombatItem combatItem)
                {
                    target.Equip(new StockedCombatItem(combatItem, 1),1);
                }
                else if (target.CombatItem2 == null&&potion is IEquipableCombatItem combatItem2)
                {
                    target.Equip(new StockedCombatItem(combatItem2, 1),2);
                }
                else
                {
                    Player.Instance.Party.Convoy.AddItem(potion);
                }
            }
          
        }

        public override void Deactivate(Unit user, int skillLevel)
        {
            throw new System.NotImplementedException();
        }

      
      
        public override List<EffectDescription> GetEffectDescription(int level)
        {
            return new List<EffectDescription>();
        }
        
    }
}