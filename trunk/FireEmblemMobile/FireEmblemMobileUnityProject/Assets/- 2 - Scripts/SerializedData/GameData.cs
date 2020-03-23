using Assets.GameActors.Players;

namespace Assets.SerializedData
{
    [System.Serializable]
    public class GameData
    {
        public Player Player;
        public GameProgress GameProgress;
        public GameData(Player player, GameProgress gameProgress)
        {
            this.Player = player;
            this.GameProgress = gameProgress;
        }
    }
}