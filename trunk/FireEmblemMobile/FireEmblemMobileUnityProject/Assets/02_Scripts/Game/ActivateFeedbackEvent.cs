using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace LostGrace
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
