using Game.EncounterAreas.Encounters.Event;
using UnityEngine;

namespace Game.Dialog
{
    [CreateAssetMenu(menuName = "GameData/Events/MiniGameEvent", fileName = "MiniGameEvent")]
    public class MiniGameEvent : DialogEvent
    {
  
        [SerializeField] public MiniGame miniGame;

        public override void Action()
        {
            miniGame.StartGame();
            miniGame.OnComplete -= Complete;
            miniGame.OnComplete += Complete;
        }

        void Complete()
        {
            OnComplete?.Invoke();
        }

        public override Reward GetReward()
        {
            return miniGame.GetRewards();
        }
    }
}