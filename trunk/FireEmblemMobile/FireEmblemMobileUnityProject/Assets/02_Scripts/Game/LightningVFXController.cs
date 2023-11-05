using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LostGrace
{
    public class LightningVFXController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem lightning;
        [SerializeField] private ParticleSystem dustCloud;
        [SerializeField] private ParticleSystem rootParticleSystem;
        private void Update()
        {
            // if (Input.touchCount>0&&Input.GetTouch(0).phase == TouchPhase.Began)
            // {
            //     Play(Random.Range(1,15), 90);
            // }
        }

        public void Play(float size, float rotationZ)
        {
            var main = lightning.main;
           
            main.startSize = new ParticleSystem.MinMaxCurve(size-0.5f);
            lightning.transform.localPosition = new Vector3(0, size*1.5f, 0);
            transform.localRotation = Quaternion.Euler(0,0, rotationZ);
            dustCloud.transform.rotation= Quaternion.identity;//Quaternion.Euler(0,0, rotationZ-90);
            rootParticleSystem.Play(true);
           
            
        }
    }
}
