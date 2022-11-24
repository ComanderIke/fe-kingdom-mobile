using Game.GameActors.Items.Weapons;

public class QuestReward
{
    public QuestRewardType RewardType;
    public int rewardAmount;
    public EquipableItemBP RewardItemBp;

    public QuestReward(QuestRewardType rewardType, int rewardAmount, EquipableItemBP rewardItemBp)
    {
        RewardType = rewardType;
        this.rewardAmount = rewardAmount;
        this.RewardItemBp = rewardItemBp;
    }
}public enum QuestRewardType
{
    Gold,
    Exp,
    Item
}
