using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.WorldMapStuff.Model;

public class Inn
{
    public List<ShopItem> shopItems = new List<ShopItem>();
    public Quest quest;
    public UnitBP recruitableCharacter;
    public int drinkCost=25;
    public int eatCost=50;
    public int specialCost=80;

    public Inn(Quest quest, UnitBP recruitableCharacter)
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

    public void Drink(Unit unit)
    {
        unit.Heal((int)((unit.MaxHp/100f)*50f));
        Player.Instance.Party.money -= drinkCost;
    }

    public void Eat(Unit unit)
    {
        unit.Heal((unit.MaxHp));
        

        Player.Instance.Party.money -= eatCost;
    }

    public void Rest(Unit unit)
    {
        unit.Heal((int)((unit.MaxHp/100f)*25f));
        
    }

    public void Special(Unit unit)
    {
        unit.Heal((int)((unit.MaxHp/100f)*25f));
        Player.Instance.Party.money -= specialCost;
    }
}