using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[ExecuteInEditMode]
public class AttackTrail : MonoBehaviour
{
    public Material material;
    [Range(0,1)]
    public float time = 0;

    public float duration = 1.2f;

    public float transInSpeed = 4.0f;
    public float transOutSpeed = 2.0f;
    private float elapsed = 0;

    private bool active = false;
    // Start is called before the first frame update
    private void OnEnable()
    {
        elapsed = 0;
        
        StartCoroutine(TransIn());
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
        if (!active)
            return;
        elapsed += Time.deltaTime;
        if (elapsed > duration)
        {
            active = false;
            StartCoroutine(TransOut());
        }
        
    }

    void Animate()
    {
        material.SetFloat("Time", time);
    }

    IEnumerator TransIn()
    {

        while (time > 0)
        {
            time -= Time.deltaTime * transInSpeed;
            Animate();
            yield return null;
        }
        active = true;
    }
    IEnumerator TransOut()
    {

        while (time < 1)
        {
            time += Time.deltaTime * transOutSpeed;
            Animate();
            yield return null;
        }
    }
}
