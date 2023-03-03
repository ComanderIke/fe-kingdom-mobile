using Game.GameActors.Players;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Events/MiniGameEvent", fileName = "MiniGameEvent")]
public class MiniGameEvent : DialogEvent
{
  
    [SerializeField] public MiniGame miniGame;

    public override void Action()
    {
        miniGame.StartGame();
    }
}