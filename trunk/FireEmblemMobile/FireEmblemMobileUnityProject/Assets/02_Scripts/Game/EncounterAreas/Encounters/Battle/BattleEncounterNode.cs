using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameResources;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using Menu;
using UnityEngine;

public enum BattleType
{
    Normal, Elite, Boss, Final
}
public class BattleEncounterNode : EncounterNode
{
    private Scenes levelindex;
    public BattleMap BattleMap;
    public BattleType BattleType;

    public BattleEncounterNode(Scenes levelIndex, BattleMap battleMap, BattleType battleType,List<EncounterNode> parents,int depth, int childIndex,string label, string description, Sprite sprite) : base(parents,depth, childIndex, label, description, sprite)
    {
        this.levelindex = levelIndex;
        this.BattleMap = battleMap;
        this.BattleType = battleType;
    }

    public override void Init()
    {
        MyDebug.LogTest("INIT BATTLE ENCOUNTER NODE");
        if (Player.Instance.Party.EncounterComponent.MovedEncounterIds.Contains(base.GetId()))
        {
            MyDebug.LogTest("NOT VISITED");
            gameObject.GetComponent<BattleEncounterController>().HideSprite();
        }
            
    }
    public override void Activate(Party party)
    {
        MyDebug.LogLogic("Visiting Battle" );
        base.Activate(party);
        //TODO Continue should be called after the battle has ended.
        Continue();
        base.gameObject.GetComponent<BattleEncounterController>().Activate();
        if (BattleMap == null)
        {
            BattleMap = GameBPData.Instance.GetRandomMap(BattleType);
            int cnt = 0;
            while (party.HasVisitedMap(BattleMap)&& cnt<100)
            {
                BattleMap = GameBPData.Instance.GetRandomMap(BattleType);;
                cnt++;
            }
        }
       

        MyDebug.LogTest("Visited BattleMap: " + BattleMap.name);
        party.VisitedMaps.Add(BattleMap);
        GameSceneController.Instance.LoadBattleLevel(levelindex, BattleMap, BattleType); //, this);
    }
}