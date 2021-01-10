

namespace SerializedData
{
    [System.Serializable]
    public class GameData
    {
        public PlayerData Player;
        public GameProgress GameProgress;
        public GameData(PlayerData player, GameProgress gameProgress)
        {
            Player = player;
            GameProgress = gameProgress;
        }
    }
}