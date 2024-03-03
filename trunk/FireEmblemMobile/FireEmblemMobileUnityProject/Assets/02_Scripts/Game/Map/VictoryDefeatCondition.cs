using Game.Manager;
using UnityEngine;

namespace Game.Map
{
    [CreateAssetMenu(fileName = "VictoryDefeatCondition", menuName = "GameData/ChapterConditions", order = 0)]
    public class VictoryDefeatCondition : ScriptableObject
    {
        public string description;
        public bool victory;
        public ConditionType type;

        public bool CheckCondition(FactionManager factionManager)
        {
     
            if (victory)
            {
                switch (type)
                {
                    case ConditionType.Route: 
                        foreach (var p in factionManager.Factions)
                        {
                            
                            
                            if (!p.IsPlayerControlled )
                            {
                                if (!p.IsAlive())
                                {
                                    return true;
                                }
                            }
                        }

                        return false;
                    case ConditionType.KillBoss:
                    {
                        bool allBossesDead = true;
                        foreach (var p in factionManager.Factions)
                        {
                            if (!p.IsPlayerControlled )
                            {
                                foreach (var unit in p.Units)
                                {
                                    if (unit.IsAlive()&& unit.IsBoss)
                                    {
                                   
                                        allBossesDead=false;
                                    }
                                }
                              
                               
                            }
                        }

                        return allBossesDead;
                        break;
                    }
                    case ConditionType.Seize: break;
                }
                
            }
            else
            {
                switch (type)
                {
                    case ConditionType.Route: foreach (var p in factionManager.Factions)
                        {

                            if (p.IsPlayerControlled && !p.IsAlive())
                            {
                                return true;
                            }
                        }

                        return false;
                    case ConditionType.KillBoss: break;
                    case ConditionType.Seize: break;
                }
            }
            
            

            return false;
        }
    }
    public enum ConditionType{
        Route,
        KillBoss,
        Seize
    }
}