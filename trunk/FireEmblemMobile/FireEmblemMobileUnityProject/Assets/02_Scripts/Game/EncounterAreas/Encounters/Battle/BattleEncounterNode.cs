using Game.GameActors.Players;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using Menu;
using UnityEngine;

public class BattleEncounterNode : EncounterNode
{
    private Scenes levelindex;
    private EnemyArmyData enemyArmyData;
    
    public BattleEncounterNode(Scenes levelIndex, EnemyArmyData enemyArmyData, EncounterNode parent,int depth, int childIndex,string label, string description, Sprite sprite) : base(parent,depth, childIndex, label, description, sprite)
    {
        this.levelindex = levelIndex;
        this.enemyArmyData = enemyArmyData;
    }

    public override void Activate(Party party)
    {
        Continue();
        base.gameObject.GetComponent<BattleEncounterController>().Activate();
        GameSceneController.Instance.LoadBattleLevel(levelindex, enemyArmyData, this);
    }
}