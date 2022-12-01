using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.WorldMapStuff.Model;

public class Inn
{
    private List<ShopItem> shopItems = new List<ShopItem>();
    private Quest quest;
    private UnitBP recruitableCharacter;
    private int restCost = 0;
    private int drinkCost=20;
    private int eatCost=30;
    private int specialCost=80;
    private int restHeal = 15;
    private int drinkHeal = 20;
    private int eatHeal = 35;
    private List<Unit> alreadyAte;
    private List<Unit> alreadyDrank;
    private List<Unit> alreadyRested;


    public Inn(Quest quest, UnitBP recruitableCharacter):base()
    {
        this.quest = quest;
        this.recruitableCharacter = recruitableCharacter;
      
    }
    public Inn()
    {
        alreadyAte = new List<Unit>();
        alreadyDrank = new List<Unit>();
        alreadyRested = new List<Unit>();
    }

    public void AddItem(ShopItem item)
    {
        shopItems.Add(item);
    }

    public void Drink(Unit unit)
    {
        unit.Heal((int)((unit.MaxHp/100f)*50f));
        alreadyDrank.Add(unit);
        Player.Instance.Party.Money -= drinkCost;
    }

    public void Eat(Unit unit)
    {
        unit.Heal((unit.MaxHp));
        alreadyAte.Add(unit);
        Player.Instance.Party.Money -= eatCost;
    }

    public void Rest(Unit unit)
    {
        alreadyRested.Add(unit);
        unit.Heal((int)((unit.MaxHp/100f)*25f));
        
    }

    public bool CanUnitEat(Unit unit)
    {
        return !alreadyAte.Contains(unit);
    }
    public bool CanUnitDrink(Unit unit)
    {
        return !alreadyDrank.Contains(unit);
    }
    public bool CanUnitRest(Unit unit)
    {
        return !alreadyRested.Contains(unit);
    }
    
    public void Special(Unit unit)
    {
        unit.Heal((int)((unit.MaxHp/100f)*25f));
        Player.Instance.Party.Money -= specialCost;
    }
    public int GetRestPrice()
    {
        return restCost;
    }
    public int GetDrinkPrice()
    {
        return drinkCost;
    }

    public int GetEatPrice()
    {
        return eatCost;
    }
    public int GetRestHeal()
    {
        return restHeal;
    }
    public int GetDrinkHeal()
    {
        return drinkHeal;
    }

    public int GetEatHeal()
    {
        return eatHeal;
    }
}