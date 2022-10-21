using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class AnimateHDRMaterial : MonoBehaviour
    {
        [SerializeField] private Material material;
        [SerializeField] private float speed;
        [SerializeField] private float maxIntensity=2.5f;
        private float time;
        
        private static readonly int Color1 = Shader.PropertyToID("_Color");

        private void Update()
        {
            time += Time.deltaTime*speed;
            if (time >= 1)
                speed *= -1;
            if (time <= 0)
                speed = Mathf.Abs(speed);
            material.SetColor(Color1, Color.Lerp(new Color(1,1,1,1),new Color(maxIntensity,maxIntensity,maxIntensity,1), time));
        }
    }
}
