using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/BattleRewardConfig", fileName="battleRewardConfig")]
    public class BattleRewardConfig : ScriptableObject
    {
        [SerializeField]private int bexpPerEnemy;
        [SerializeField]private int goldPerEnemy;
        [SerializeField]private int bexpPerEliteEnemy;
        [SerializeField]private int goldPerEliteEnemy;
        [SerializeField]private int bexpPerTurnLeft;
        [SerializeField]private int goldPerTurnLeft;
        [SerializeField]private int defaultTurnCount;


        public int BexpPerEnemy => bexpPerEnemy;

        public int GoldPerEnemy => goldPerEnemy;

        public int BexpPerEliteEnemy => bexpPerEliteEnemy;

        public int GoldPerEliteEnemy => goldPerEliteEnemy;

        public int BexpPerTurnLeft => bexpPerTurnLeft;

        public int GoldPerTurnLeft => goldPerTurnLeft;

        public int DefaultTurnCount => defaultTurnCount;
    }
}