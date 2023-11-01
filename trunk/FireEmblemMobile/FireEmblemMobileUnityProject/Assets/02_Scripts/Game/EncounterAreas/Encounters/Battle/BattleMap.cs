using Game.Grid;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/BattleMap", fileName="BattleMap1")]
public class BattleMap : ScriptableObject
{
    public GameObject mapPrefab;
    public VictoryDefeatCondition[] victoryDefeatConditions;
    public new string name;
    public int width;
    public int height;
    public int playerUnitsSpawnPoints = 4;
    public int initialEnemyCount = 5;
}