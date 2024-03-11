using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class UIRageMeterPoint : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite ragePointFullSprite;
        [SerializeField] private MMF_Player activateFeedbacks;

        public void Activate()
        {
            Fill();
            activateFeedbacks.PlayFeedbacks();
        }
        public void Fill()
        {
            image.sprite = ragePointFullSprite;
        }
    }
}