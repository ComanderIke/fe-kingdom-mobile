using TMPro;
using UnityEngine;

namespace Game.GUI
{
    public class RewardLineUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TextMeshProUGUI goldAmount;
        [SerializeField] private TextMeshProUGUI expAmount;
        [SerializeField] private TextMeshProUGUI rewardCount;
        [SerializeField] private Animator uiIdleAnimation;

        public void SetValues(string labelText, int goldAmount, int expAmount, int rewardCount,
            AnimatorOverrideController overrideController)
        {
            this.label.text = labelText;
            
            if (goldAmount == 0)
            {
                this.goldAmount.gameObject.SetActive(false);
            }
            else
            {
                this.goldAmount.gameObject.SetActive(true);
                this.goldAmount.text = ""+goldAmount;
            }
            if (expAmount == 0)
            {
                this.expAmount.gameObject.SetActive(false);
            }
            else
            {
                this.expAmount.gameObject.SetActive(true);
                this.expAmount.text = ""+this.expAmount;
            }
            if (rewardCount == 0)
            {
                this.rewardCount.gameObject.SetActive(false);
            }
            else
            {
                this.rewardCount.gameObject.SetActive(true);
                this.rewardCount.text = ""+rewardCount;
            }
            if (overrideController == null)
                uiIdleAnimation.gameObject.SetActive(false);
            else
            {
                uiIdleAnimation.runtimeAnimatorController = overrideController;
            }
        }
    }
}
