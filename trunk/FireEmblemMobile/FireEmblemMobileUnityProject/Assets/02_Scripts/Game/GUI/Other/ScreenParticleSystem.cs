using Game.Utility;
using UnityEngine;

namespace Game.GUI.Other
{
    public class ScreenParticleSystem : MonoBehaviour
    {
        [SerializeField] private GameObject onTouchDownEffect = default;
        [SerializeField] private GameObject onTouchEffect = default;
       // [SerializeField] private Camera psCamera = default;
        [SerializeField] private Transform parent = default;

        private RectTransform screenParticles;

        private void OnDisable()
        {
            //TODO if turned into a system Deactivate all events and stuff
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SpawnTouchDownParticles();
                SpawnTouchParticles();
            }

            if (ReferenceEquals(screenParticles, null))
                return;
            if (Input.GetMouseButton(0))
            {
               
                screenParticles.anchoredPosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                screenParticles.gameObject.AddComponent<ParticleSystemAutoDestroy>();
            }
        }

        private void SpawnTouchDownParticles()
        {
            var position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //position.z = 1;
            var ps = Instantiate(onTouchDownEffect, parent, false);
            ps.GetComponent<RectTransform>().anchoredPosition = position;
            ps.AddComponent<ParticleSystemAutoDestroy>();
           

            ps.GetComponent<ParticleSystem>().Play(true);
        }
        private void SpawnTouchParticles()
        {
            var position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //position.z = 1;
            screenParticles = onTouchEffect.GetComponent<RectTransform>();
            screenParticles.anchoredPosition = position;
            var ps = Instantiate(onTouchEffect, parent, false);
            screenParticles = ps.GetComponent<RectTransform>();



            ps.GetComponent<ParticleSystem>().Play(true);
        }
    }
}