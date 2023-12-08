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
        public float[] rarityIncrease;
       
        public override void Activate(Unit target, int level)
        {
            // check if objects in the way
            if (randomPotion)
            {
                
                var potion = GameBPData.Instance.GetRandomPotion(rarityIncrease[level]);
                ToolTipSystem.Show(potion, new Vector2(Screen.width/2f, Screen.height/2f), true, true);
                if (target.CombatItem1 == null&&potion is IEquipableCombatItem combatItem)
                {
                    target.Equip(new StockedCombatItem(combatItem, 1),1);
                }
                // else if (target.CombatItem2 == null&&potion is IEquipableCombatItem combatItem2)
                // {
                //     target.Equip(new StockedCombatItem(combatItem2, 1),2);
                // }
                else
                {
                    Player.Instance.Party.AddItem(potion);
                }
                
                
            }

          

        }

        public override void Deactivate(Unit user, int skillLevel)
        {
            throw new System.NotImplementedException();
        }

      
      
        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            var list = new List<EffectDescription>();
            string value = ""+rarityIncrease[level]*100+"%";
            string upg = value;
            if(level+1 < rarityIncrease.Length)
               upg =""+ rarityIncrease[level + 1]*100+"%";
            list.Add(new EffectDescription("Rare Item: ", value, upg));
            return list;
        }
        
    }
}