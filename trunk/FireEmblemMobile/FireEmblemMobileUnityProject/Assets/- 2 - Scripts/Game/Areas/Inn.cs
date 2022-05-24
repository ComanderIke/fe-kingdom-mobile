using System.Collections.Generic;
using Game.GameActors.Units;
using Game.WorldMapStuff.Model;

public class Inn
{
    public List<ShopItem> shopItems = new List<ShopItem>();
    public Quest quest;
    public Unit recruitableCharacter;
    public int drinkCost=25;
    public int eatCost=50;
    public int specialCost=80;

    public Inn(Quest quest, Unit recruitableCharacter)
    {
        this.quest = quest;
        this.recruitableCharacter = recruitableCharacter;
    }
    public Inn()
    {

    }

    public void AddItem(ShopItem item)
    {
        shopItems.Add(item);
    }

    public void Drink(Party party)
    {
        foreach (var member in party.members)
        {
            member.Heal((int)((member.Stats.MaxHp/100f)*50f));
        }
        party.money -= drinkCost;
    }

    public void Eat(Party party)
    {
        foreach (var member in party.members)
        {
            member.Heal((member.Stats.MaxHp));
        }

        party.money -= eatCost;
    }

    public void Rest(Party party)
    {
        foreach (var member in party.members)
        {
            member.Heal((int)((member.Stats.MaxHp/100f)*25f));
        }
    }

    public void Special(Party party)
    {
        throw new System.NotImplementedException();
    }
}