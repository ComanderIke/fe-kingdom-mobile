using UnityEngine;

namespace Game.Grid
{
    [CreateAssetMenu(fileName = "Chapter1", menuName = "GameData/Chapter", order = 0)]
    public class Chapter : ScriptableObject
    {
        public VictoryDefeatCondition[] victoryDefeatConditions;
        public new string name;
    }
}