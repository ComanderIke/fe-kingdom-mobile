using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AnimationTimer : MonoBehaviour
{
    public float normalizedTime;

    void Update()
    {
        normalizedTime += Time.deltaTime;
        normalizedTime = normalizedTime % 1.0f;
    }
}
