using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Utility;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace LostGrace
{
    public class CritCutInnController : MonoBehaviour
    {
        [SerializeField]private GameObject cameraScene;

        [SerializeField] private Image faceSprite;

        [SerializeField] private MMF_Player feedbacks;

        [SerializeField] private ParticleSystem particleSystem;
        [SerializeField] private float critDuration=1.25f;

        public void Show(Unit unit)
        {
            var localRot = this.transform.localRotation;
            if (unit.Faction.IsPlayerControlled)
                localRot = Quaternion.Euler(localRot.eulerAngles.x, 0, localRot.eulerAngles.z);
            else
                localRot = Quaternion.Euler(localRot.eulerAngles.x, 180, localRot.eulerAngles.z);
            transform.localRotation = localRot;
            gameObject.SetActive(true);
            cameraScene.SetActive(true);
            faceSprite.sprite = unit.FaceSprite;
            MonoUtility.InvokeNextFrame(() =>
            {
                feedbacks.PlayFeedbacks();
            });
            AnimationQueue.Add(()=>MonoUtility.DelayFunction(()=>Hide(), critDuration));
            
            //TODO set background and particle color special to units and special for enemies / bosses
        }

        void Hide()
        {
            AnimationQueue.OnAnimationEnded?.Invoke();
            feedbacks.StopFeedbacks();
            cameraScene.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
