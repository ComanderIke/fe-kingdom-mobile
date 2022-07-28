public class Quest
{
    public string description;
    public QuestReward Reward;

    public Quest(string description, QuestReward reward)
    {
        this.description = description;
        Reward = reward;
    }
}