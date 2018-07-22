using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "GameData/Unit/Stats",fileName ="UnitStats")]
public class Stats : ScriptableObject
{
    public int MaxHP;
    public int Speed;
    public int Defense;
    public int Attack;
    public int Accuracy;
    public int Spirit;
    public int MaxSP;
    public int MoveRange;
    public List<int> AttackRanges;

    public int GetMaxAttackRange()
    {
        int max = 0;
        foreach (int attack in AttackRanges)
        {
            if (attack > max)
                max = attack;
        }
        return max;
    }
}

