using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    [SerializeField]
    Stats saberStats;
    [SerializeField]
    Stats mammothStats;
    [SerializeField]
    Stats defaultStats;
    [SerializeField]
    Stats swordFighterStats;
    [SerializeField]
    Stats archerStats;
    [SerializeField]
    Stats axeFighterStats;
    [SerializeField]
    Stats spearFighterStats;

    [SerializeField]
    List<AttackType> attackTypes;
    [SerializeField]
    List<DefenseType> defenseTypes;

    public List<AttackType> AttackTypes
    {
        get
        {
            List<AttackType> ret = new List<AttackType>();
            for(int i=0; i < attackTypes.Count; i++) 
            {
                AttackType at = GameObject.Instantiate(attackTypes[i]);
                at.name = attackTypes[i].name;
                ret.Add(at);
            }
            return ret;
        }
    }
    public List<DefenseType> DefenseTypes
    {
        get
        {
            List<DefenseType> ret = new List<DefenseType>();
            for (int i = 0; i < defenseTypes.Count; i++)
            {
                DefenseType dt = GameObject.Instantiate(defenseTypes[i]);
                dt.name = defenseTypes[i].name;
                ret.Add(dt);
            }
            return ret;
        }
    }

    public Stats SaberStats
    {
        get
        {
            return GameObject.Instantiate(saberStats);
        }
    }
    public Stats DefaultStats
    {
        get
        {
            return GameObject.Instantiate(defaultStats);
        }
    }
    public Stats MammothStats
    {
        get
        {
            return GameObject.Instantiate(mammothStats);
        }
    }
    public Stats ArcherStats
    {
        get
        {
            return GameObject.Instantiate(archerStats);
        }
    }
    public Stats SwordFighterStats
    {
        get
        {
            return GameObject.Instantiate(swordFighterStats);
        }
    }
    public Stats AxeFighterStats
    {
        get
        {
            return GameObject.Instantiate(axeFighterStats);
        }
    }
    public Stats SpearFighterStats
    {
        get
        {
            return GameObject.Instantiate(spearFighterStats);
        }
    }
}

