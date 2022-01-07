using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using Menu;

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
        Continue();
        GameSceneController.Instance.LoadBattleLevel(levelindex, enemyArmyData, this);
    }
}