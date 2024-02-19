using UnityEngine;
using UnityEngine.Serialization;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/BattleRewardConfig", fileName="battleRewardConfig")]
    public class BattleRewardConfig : ScriptableObject
    {
        [FormerlySerializedAs("bexpPerEnemy")] [SerializeField]private int gracePerEnemy;
        [SerializeField]private int goldPerEnemy;
        [FormerlySerializedAs("bexpPerEliteEnemy")] [SerializeField]private int gracePerEliteEnemy;
        [SerializeField]private int goldPerEliteEnemy;
        [FormerlySerializedAs("bexpPerTurnLeft")] [SerializeField]private int gracePerTurnLeft;
        [SerializeField]private int goldPerTurnLeft;
        [SerializeField]private int defaultTurnCount;
        [SerializeField] private int victoryGold;
        [SerializeField] private int victoryGrace;


        public int GracePerEnemy => gracePerEnemy;

        public int GoldPerEnemy => goldPerEnemy;

        public int GracePerEliteEnemy => gracePerEliteEnemy;

        public int GoldPerEliteEnemy => goldPerEliteEnemy;

        public int GracePerTurnLeft => gracePerTurnLeft;

        public int GoldPerTurnLeft => goldPerTurnLeft;

        public int DefaultTurnCount => defaultTurnCount;
        public int VictoryGold => victoryGold;
        public int VictoryGrace => victoryGrace;
    }
}