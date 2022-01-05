using System.Collections.Generic;
using Game.GameActors.Units;

public class Inn
{
    public List<ShopItem> shopItems = new List<ShopItem>();
    public Quest quest;
    public Unit recruitableCharacter;

    public Inn(Quest quest, Unit recruitableCharacter)
    {
        this.quest = quest;
        this.recruitableCharacter = recruitableCharacter;
    }

    public void AddItem(ShopItem item)
    {
        shopItems.Add(item);
    }
}