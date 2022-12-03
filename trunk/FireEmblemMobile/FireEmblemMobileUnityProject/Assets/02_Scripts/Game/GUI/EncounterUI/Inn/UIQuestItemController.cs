using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestItemController : UIButtonController
{
    public TextMeshProUGUI rewardText;

    public Image rewardIcon;

    public Sprite goldSprite;

    public Sprite expSprite;

    public Sprite QuestSprite;
    // Start is called before the first frame update
    public void SetValues(Quest quest)
    {
        rewardText.SetText(""+quest.Reward.rewardAmount);
        switch (quest.Reward.RewardType)
        {
            case QuestRewardType.Exp: rewardIcon.sprite = expSprite;break;
            case QuestRewardType.Gold:
                rewardIcon.sprite = goldSprite;break;
            case QuestRewardType.Item: rewardIcon.sprite = quest.Reward.RewardItemBp.sprite;break;
        }
        SetValues(QuestSprite, "Accept");
    }
}

