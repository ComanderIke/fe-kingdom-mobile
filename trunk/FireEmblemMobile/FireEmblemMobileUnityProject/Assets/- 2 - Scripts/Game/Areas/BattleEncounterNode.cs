using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using Menu;

public class BattleEncounterNode : EncounterNode
{
    private Scenes levelindex;
    private EnemyArmyData enemyArmyData;
    
    public BattleEncounterNode(Scenes levelIndex, EnemyArmyData enemyArmyData, EncounterNode parent,int depth, int childIndex) : base(parent,depth, childIndex)
    {
        this.levelindex = levelIndex;
        this.enemyArmyData = enemyArmyData;
    }

    public override void Activate(Party party)
    {
        Continue();
        GameSceneController.Instance.LoadBattleLevel(levelindex, enemyArmyData, this);
    }
}