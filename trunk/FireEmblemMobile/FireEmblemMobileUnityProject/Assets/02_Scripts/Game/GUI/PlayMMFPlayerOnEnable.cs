using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.GUI
{
    public class PlayMMFPlayerOnEnable : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponent<MMF_Player>().Initialization();
            GetComponent<MMF_Player>().PlayFeedbacks();
        }
    }
}
