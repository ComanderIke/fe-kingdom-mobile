using Game.WorldMapStuff.Model;
using UnityEngine;

public class BattleEncounterNode : EncounterNode
{
    private Scenes levelindex;
    private EnemyArmyData enemyArmyData;
    
    public BattleEncounterNode(Scenes levelIndex, EnemyArmyData enemyArmyData, EncounterNode parent) : base(parent)
    {
        this.levelindex = levelIndex;
        this.enemyArmyData = enemyArmyData;
    }

    public override void Activate()
    {
        Debug.Log("ACtivate BattleEncoutnerNode");
    }
}
public class StartEncounterNode : EncounterNode
{
    public StartEncounterNode() : base(null)
    {

    }

    public override void Activate()
    {
        Debug.Log("Activate StartEncounterNode");
    }
}
public class InnEncounterNode : EncounterNode
{
    public InnEncounterNode(EncounterNode parent) : base(parent)
    {

    }

    public override void Activate()
    {
        Debug.Log("Activate InnEncounterNode");
    }
}
public class MerchantEncounterNode : EncounterNode
{
    public MerchantEncounterNode(EncounterNode parent) : base(parent)
    {

    }

    public override void Activate()
    {
        Debug.Log("Activate MerchantEncounterNode");
    }
}
public class ChurchEncounterNode : EncounterNode
{
    public ChurchEncounterNode(EncounterNode parent) : base(parent)
    {

    }

    public override void Activate()
    {
        Debug.Log("Activate ChurchEncounterNode");
    }
}
public class SmithyEncounterNode : EncounterNode
{
    public SmithyEncounterNode(EncounterNode parent) : base(parent)
    {

    }

    public override void Activate()
    {
        Debug.Log("Activate SmithyEncounterNode");
    }
}
public class EventEncounterNode : EncounterNode
{
    public EventEncounterNode(EncounterNode parent) : base(parent)
    {

    }

    public override void Activate()
    {
        Debug.Log("Activate EventEncounterNode");
    }
}