using System.Collections.Generic;
using Game.GameActors.Players;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using Menu;
using UnityEngine;

public class BattleEncounterNode : EncounterNode
{
    private Scenes levelindex;
    public EnemyArmyData enemyArmyData;
    
    public BattleEncounterNode(Scenes levelIndex, EnemyArmyData enemyArmyData, List<EncounterNode> parents,int depth, int childIndex,string label, string description, Sprite sprite) : base(parents,depth, childIndex, label, description, sprite)
    {
        this.levelindex = levelIndex;
        this.enemyArmyData = enemyArmyData;
    }

    public override void Activate(Party party)
    {
        base.Activate(party);
        //TODO Continue should be called after the battle has ended.
        Continue();
        base.gameObject.GetComponent<BattleEncounterController>().Activate();
        GameSceneController.Instance.LoadBattleLevel(levelindex, enemyArmyData); //, this);
    }
}