﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTextController : MonoBehaviour {

    [SerializeField]
    private GameObject popUpTextRedPrefab;
    [SerializeField]
    private GameObject popUpTextGreenPrefab;
    [SerializeField]
    private Canvas attackCanvas;
    [SerializeField]
    private Canvas defendCanvas;


    public void CreateAttackPopUpTextRed(string text, Transform location)
    {
        GameObject instance = Instantiate(popUpTextRedPrefab);
        //Vector2 screenPosition = Camera.main.WorldToScreenPoint(location.position);
        instance.transform.SetParent(attackCanvas.transform, false);
        instance.transform.position = location.position;
        float randomX = Random.Range(-1.0f, 1.0f) * 0.5f;
        float randomY = Random.Range(-1.0f, 1.0f) * 0.5f;
        instance.transform.Translate(new Vector3(randomX, randomY,0));
        instance.GetComponentInChildren<FloatingText>().SetText(text);
    }
    public void CreateAttackPopUpTextGreen(string text, Transform location)
    {
        GameObject instance = Instantiate(popUpTextGreenPrefab);
        //Vector2 screenPosition = Camera.main.WorldToScreenPoint(location.position);
        instance.transform.SetParent(attackCanvas.transform, false);
        instance.transform.position = location.position;
        instance.GetComponentInChildren<FloatingText>().SetText(text);
    }
    public void CreateDefendPopUpTextRed(string text, Transform location)
    {
        GameObject instance = Instantiate(popUpTextRedPrefab);
        //Vector2 screenPosition = Camera.main.WorldToScreenPoint(location.position);
        instance.transform.SetParent(defendCanvas.transform, false);
        instance.transform.position = location.position;
        float randomX = Random.Range(-1.0f, 1.0f) * 0.5f;
        float randomY = Random.Range(-1.0f, 1.0f) * 0.5f;
        instance.transform.Translate(new Vector3(randomX, randomY, 0));
        instance.GetComponentInChildren<FloatingText>().SetText(text);
    }
    public void CreateDefendPopUpTextGreen(string text, Transform location)
    {
        GameObject instance = Instantiate(popUpTextGreenPrefab);
        //Vector2 screenPosition = Camera.main.WorldToScreenPoint(location.position);
        instance.transform.SetParent(defendCanvas.transform, false);
        instance.transform.position = location.position;
        instance.GetComponentInChildren<FloatingText>().SetText(text);
    }
}
