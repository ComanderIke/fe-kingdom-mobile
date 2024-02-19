using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.Grid;
using UnityEngine;

[System.Serializable]
public class RewardItem
{
    public float chance = 1.0f;
    public int count = 1;
    public ItemBP item;

}
[CreateAssetMenu(menuName = "GameData/BattleMap", fileName="BattleMap1")]
public class BattleMap : ScriptableObject
{
    public GameObject mapPrefab;
    public VictoryDefeatCondition[] victoryDefeatConditions;
    public new string name;
    [SerializeField]private int width;
    [SerializeField]private int height;
    public int playerUnitsSpawnPoints = 4;
    private bool init = false;
    public int victoryGold;
    public int victoryGrace;
    public int turnCount;
    public List<RewardItem> victoryItems;

    private void OnEnable()
    {
        init = false;
    }

    void Init()
    {
        width = mapPrefab.GetComponentInChildren<GridBuilder>().GetWidth();
        height = mapPrefab.GetComponentInChildren<GridBuilder>().GetHeight();
        MyDebug.LogTest("INIT BATTLE MAP:" +width+" "+height);
        init = true;
    }
    public int GetWidth()
    {
        if (!init)
            Init();
        return width;
    }
    public int GetHeight()
    {
        if (!init)
            Init();
        return height;
    }
}