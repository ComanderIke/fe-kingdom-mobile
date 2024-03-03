using Game.EncounterAreas.Model;
using Game.GameActors.Player;
using Game.Manager;
using UnityEngine;

namespace Game.GUI
{
    public class CheatWinBattleButtonScript : MonoBehaviour
    {
       
        void Start()
        {
        
        }

        void Update()
        {
        
        }

        public void Clicked()
        {
            Player.Instance.LastBattleOutcome = BattleOutcome.Victory;
            GameSceneController.Instance.LoadEncounterAreaAfterBattle(true);
        }
    }
}
