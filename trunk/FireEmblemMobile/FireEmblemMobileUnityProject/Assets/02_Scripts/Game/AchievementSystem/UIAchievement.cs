using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Game.GameActors.Units;
using Game.GameResources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines the logic behind a single achievement on the UI
/// </summary>
public class UIAchievement : MonoBehaviour
{
    public TextMeshProUGUI Title, Description;

    [SerializeField] private TextMeshProUGUI rewardText;

    [SerializeField] private CanvasGroup rewardCanvasGroup;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private Vector2 unitRewardSize = new Vector2(80, 80);
    [SerializeField] private TextMeshProUGUI claimText;
    [SerializeField] private Sprite ClaimSprite;
    [SerializeField] private Sprite greyClaimSprite;
    public Image ClaimIcon;

    public Image BackgroundImage;
    [SerializeField] private Sprite BackgroundSpriteAchieved;
    [SerializeField] private Sprite BackgroundSpriteNormal;

    [SerializeField] private CanvasGroup canvasGroup;

    private string key;
        //, OverlayIcon, ProgressBar;
   // public GameObject SpoilerOverlay;
   // public TextMeshProUGUI SpoilerText;
    [HideInInspector]public AchievenmentStack AS;

    /// <summary>
    /// Destroy object after a certain amount of time
    /// </summary>
    public void StartDeathTimer ()
    {
        StartCoroutine(Wait());
    }

    public void ClaimClicked()
    {
        AchievementManager.instance.Claim(key);
    }
    /// <summary>
    /// Add information  about an Achievement to the UI elements
    /// </summary>
    public void Set (AchievementInfromation Information, AchievementState State)
    {
        // if(Information.Spoiler && !State.Achieved)
        // {
        //     SpoilerOverlay.SetActive(true);
        //     SpoilerText.text = AchievementManager.instance.SpoilerAchievementMessage;
        // }
        // else
        // {
        key = Information.Key;
            Title.text = Information.DisplayName;
            Description.text = Information.Description;
            claimText.text = State.Claimed?"Claimed":"Claim";
            ClaimIcon.sprite = State.Achieved ? ClaimSprite : greyClaimSprite;
            canvasGroup.alpha = State.Claimed ? .3f : 1f;
            canvasGroup.interactable = !State.Claimed;
            BackgroundImage.sprite = State.Achieved ? State.Claimed?BackgroundSpriteNormal: BackgroundSpriteAchieved : BackgroundSpriteNormal;
            rewardText.text =""+ Information.RewardAmount;
            rewardCanvasGroup.alpha = State.Achieved?1f:.5f;
            switch (Information.RewardType)
            {
                case RewardType.Grace:
                {
                    rewardIcon.sprite = GameAssets.Instance.visuals.Icons.Grace; break;
                }
                case RewardType.Character:
                    var u = GameBPData.Instance.GetAllPlayableUnits().First(u => u.Name == Information.RewardAmount);
                    rewardIcon.sprite = u.visuals.CharacterSpriteSet.MapSprite;
                    rewardIcon.rectTransform.sizeDelta = unitRewardSize;
                    break;
            }

            if (Information.Progression)
            {
                // float CurrentProgress = AchievementManager.instance.ShowExactProgress ? State.Progress : (State.LastProgressUpdate * Information.NotificationFrequency);
                // float DisplayProgress = State.Achieved ? Information.ProgressGoal : CurrentProgress;

                // if (State.Achieved)
                // {
                //     Percent.text = Information.ProgressGoal + Information.ProgressSuffix + " / " + Information.ProgressGoal + Information.ProgressSuffix + " (Achieved)";
                // }
                // else
                // {
                //     Percent.text = DisplayProgress + Information.ProgressSuffix +  " / " + Information.ProgressGoal + Information.ProgressSuffix;
                // }

               // ProgressBar.fillAmount = DisplayProgress / Information.ProgressGoal;
            }
            else //Single Time
            {
               // ProgressBar.fillAmount = State.Achieved ? 1 : 0;
                // Percent.text = State.Achieved ? "(Achieved)" : "(Locked)";
            }
        // }
    }

    private IEnumerator Wait ()
    {
        yield return new WaitForSeconds(AchievementManager.instance.DisplayTime);
        GetComponent<Animator>().SetTrigger("ScaleDown");
        yield return new WaitForSeconds(0.4f);
        AS.CheckBackLog();
        Destroy(gameObject);
    }
}
