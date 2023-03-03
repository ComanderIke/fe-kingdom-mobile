using System.Collections.Generic;
using Game.GameActors.Items;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Events/MiniGames/MemoryGameData", fileName="MemoryGameData")]
public class MemoryGameData: MiniGame
{
    public int MaxTries = 5;
    public List<ItemBP> items;
    public int columns=5;

    public override void StartGame()
    {
        GameObject.FindObjectOfType<MemoryMiniGame>().Show(this);
    }
}