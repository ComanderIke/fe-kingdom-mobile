using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering;

namespace LostGrace
{
    public class UIInteractionBlockerOnEnable : MonoBehaviour
    {
      private void OnEnable()
        {
            InteractionBlocker.Instance.SetActive(gameObject,true);
        }
        private void OnDisable()
        {
            InteractionBlocker.Instance.SetActive(gameObject,false);
        }

       
    }
}