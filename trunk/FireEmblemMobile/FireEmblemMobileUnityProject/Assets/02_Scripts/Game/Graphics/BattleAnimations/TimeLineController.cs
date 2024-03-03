using System;
using Game.Utility;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Game.Graphics.BattleAnimations
{
    public class TimeLineController : MonoBehaviour
    {
        public float playSpeed = 1.0f;
        public PlayableDirector playableDirector;
        public TimelineAsset cameraIntro;
        public TimelineAsset cameraIntroInverse;
        public TimelineAsset cameraZoomIn;
        public TimelineAsset cameraZoomOut;
        public CameraShake cameraShake;
        public new Camera camera;
        public float introWaitDuration = 1.2f;
        public float introWalkInPlaySpeed = 1.8f;
        public float magnitude = 0.1f;
        public float duration = 0.1f;
        public GameObject battleBackground;//BattleBackgroundOpenFieldPrefab
        public Action zoomInFinished;
        public Action zoomOutFinished;
        public float battleAnimationAlternatePositionXOffset;

        void PlayAtSpeed(PlayableDirector playableDirector, float speed)
        {
            playableDirector.RebuildGraph(); // the graph must be created before getting the playable graph
            playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(speed);
            playableDirector.Play();
        }
        private void IntroFinished()
        {
            playableDirector.Stop();
            PlayZoomIn();
        }

        public void CameraShake()
        {
            StartCoroutine(cameraShake.Shake(duration, magnitude));
        }
        public void PlayZoomIn()
        {
            playableDirector.playableAsset = cameraZoomIn;
            PlayAtSpeed(playableDirector, 1);

            Invoke("ZoomInFinished", (float)playableDirector.duration);
        }
        private void ZoomInFinished()
        {
            playableDirector.Stop();
            zoomInFinished?.Invoke();
        }

        public void Init(bool introReverse=false)
        {
            playableDirector.Stop();
            PlayIntro(introReverse);
            SetupBackground(introReverse);
        
            PlayAtSpeed(playableDirector, introWalkInPlaySpeed);
        
            Invoke("IntroFinished", introWaitDuration);
        }

        void PlayIntro(bool reverse)
        {
            playableDirector.playableAsset = reverse ? cameraIntro : cameraIntroInverse; 
            //playableDirector.playableAsset = cameraIntroInverse; 
            //camera.transform.localPosition = new Vector3( -80, camera.transform.localPosition.y, camera.transform.localPosition.z);
            camera.transform.localPosition = new Vector3(reverse ? -80 : 80, camera.transform.localPosition.y, camera.transform.localPosition.z);
        }

        private GameObject background;
        void SetupBackground(bool reverse=false)
        {
            if(background!=null)
                Destroy(background);
            background = GameObject.Instantiate(battleBackground, transform);
            background.transform.position = new Vector3(camera.transform.position.x+(!reverse?battleAnimationAlternatePositionXOffset:0), background.transform.position.y,
                background.transform.position.z);
        }


        public void PlayZoomOut()
        {
            playableDirector.playableAsset = cameraZoomOut;
            PlayAtSpeed(playableDirector, 1);
            Invoke("ZoomOutFinished", (float)playableDirector.duration);
        }

        void ZoomOutFinished()
        {
            zoomOutFinished?.Invoke();
        }
    }
}