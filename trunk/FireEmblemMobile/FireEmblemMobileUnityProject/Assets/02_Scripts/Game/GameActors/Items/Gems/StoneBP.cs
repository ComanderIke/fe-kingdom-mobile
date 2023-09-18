using UnityEngine;

namespace Game.GameActors.Items.Gems
{
    [CreateAssetMenu(menuName = "GameData/Items/Stone", fileName = "Stone")]
    public class StoneBP: ItemBP
    {
        //[SerializeField] private StoneBP upgradeTo;
        public override Item Create()
        {
            // if(upgradeTo!=null)
            //     return new Stone(name, description, cost, maxStack,sprite,rarity ,(Stone) upgradeTo.Create());
            return new Stone(name, description, cost, maxStack,sprite,rarity ,null);
        }

        
    }
}