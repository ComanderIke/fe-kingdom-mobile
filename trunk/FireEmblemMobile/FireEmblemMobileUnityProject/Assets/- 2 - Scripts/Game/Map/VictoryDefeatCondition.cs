using Game.Manager;
using UnityEngine;

namespace Game.Grid
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
                    case ConditionType.Route: foreach (var p in factionManager.Factions)
                        {

                            if (!p.IsPlayerControlled && !p.IsAlive())
                            {
                                return true;
                            }
                        }

                        return false;break;
                    case ConditionType.KillBoss: break;
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

                        return false;break;
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