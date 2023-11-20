using Game.GameActors.Items;

namespace Game.GameActors.Units
{
    public class StockedCombatItem
    {
        public IEquipableCombatItem item;
        public int stock;

        public StockedCombatItem(IEquipableCombatItem item, int stock)
        {
            this.stock = stock;
            this.item = item;
        }

        public void Use()
        {
            stock--;
            if (stock <= 0)
            {
                
            }
        }
    }
}