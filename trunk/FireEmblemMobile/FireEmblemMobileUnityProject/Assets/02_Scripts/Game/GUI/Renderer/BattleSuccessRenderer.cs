using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSuccessRenderer : MonoBehaviour, IBattleSuccessRenderer
{

    private Canvas canvas;
    [SerializeField] private float minTimeShown = 1.0f;
    private bool shown = false;
    private float time = 0;
    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Show()
    {
        canvas.enabled = true;
        shown = true;
    }

    private void Update()
    {
        if (shown)
        {
            time += Time.deltaTime;
            if (time >= minTimeShown)
            {
                if(Input.GetMouseButtonDown(0))
                    Hide();
            }
        }
    }

    public void Hide()
    {
        shown = false;
        canvas.enabled = false;
        OnFinished?.Invoke();
    }

    public event Action OnFinished;
}