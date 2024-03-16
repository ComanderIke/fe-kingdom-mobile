using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class CritCutInnController : MonoBehaviour
    {
        [SerializeField]private GameObject cameraScene;

        [SerializeField] private Image faceSprite;

        [SerializeField] private MMF_Player feedbacks;

        [SerializeField] private ParticleSystem particleSystem;

        public void Show()
        {
            //TODO set background and particle color special to units and special for enemies / bosses
        }
    }
}
