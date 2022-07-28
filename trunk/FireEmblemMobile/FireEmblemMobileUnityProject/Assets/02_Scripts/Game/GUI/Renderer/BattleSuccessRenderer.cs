using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSuccessRenderer : MonoBehaviour, IBattleSuccessRenderer
{

    private Canvas canvas;

    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Show()
    {
        canvas.enabled = true;
    }
}