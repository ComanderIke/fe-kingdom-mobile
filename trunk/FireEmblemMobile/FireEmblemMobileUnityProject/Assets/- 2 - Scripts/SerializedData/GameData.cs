

namespace SerializedData
{
    [System.Serializable]
    public class GameData
    {
        public PlayerData Player;
        public GameProgress GameProgress;
        public GameData(PlayerData player, GameProgress gameProgress)
        {
            this.Player = player;
            this.GameProgress = gameProgress;
        }
    }
}