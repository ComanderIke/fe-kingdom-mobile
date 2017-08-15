using Assets.Scripts;

[System.Serializable]
public class SaveObject
{
    public PlayerData player;
    public int levelindex;

    public SaveObject(Player p, int levelindex)
    {
        //this.player = p.GetPlayerData();
        this.levelindex = levelindex;
    }
}