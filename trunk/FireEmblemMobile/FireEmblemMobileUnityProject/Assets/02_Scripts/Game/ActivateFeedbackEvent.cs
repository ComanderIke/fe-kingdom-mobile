using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game
{
    public class ActivateFeedbackEvent : MonoBehaviour
    {
        [SerializeField] private MMF_Player mmfPlayer;

        private void OnEnable()
        {
            mmfPlayer.PlayFeedbacks();
        }
    }
}
