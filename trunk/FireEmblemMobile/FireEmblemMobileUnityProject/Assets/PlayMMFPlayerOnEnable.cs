using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace LostGrace
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
