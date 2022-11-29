using Game.GameActors.Units;
using TMPro;
using UnityEngine;

namespace LostGrace
{
    public class ResultMVPPanelUI : MonoBehaviour
    {
        [SerializeField] private Animator treasureChest;
        [SerializeField] private TextMeshProUGUI mvpName;
        [SerializeField] private Animator mvpIdleAnimation;

        public void Show(Unit mvp, GachaReward getGachaReward)
        {
            mvpName.text = mvp.Name;
            mvpIdleAnimation.runtimeAnimatorController = mvp.visuals.Prefabs.UIAnimatorController;
        }
    }
}