using Game.GameActors.Items.Weapons;

public class QuestReward
{
    public QuestRewardType RewardType;
    public int rewardAmount;
    public EquipableItem rewardItem;

    public QuestReward(QuestRewardType rewardType, int rewardAmount, EquipableItem rewardItem)
    {
        RewardType = rewardType;
        this.rewardAmount = rewardAmount;
        this.rewardItem = rewardItem;
    }
}public enum QuestRewardType
{
    Gold,
    Exp,
    Item
}
