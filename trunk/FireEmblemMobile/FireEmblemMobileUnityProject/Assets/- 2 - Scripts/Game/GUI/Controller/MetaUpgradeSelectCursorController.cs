using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MetaUpgradeSelectCursorController : MonoBehaviour
{
    public static MetaUpgradeSelectCursorController Instance;
    public GameObject selected;

    void Start()
    {
        Instance = this;
    }
    public void Show(GameObject selectedMetaSkill)
    {
        gameObject.SetActive(true);
        selected = selectedMetaSkill;
        transform.position = selected.transform.position;
    }

    private void OnEnable()
    {
        if (selected != null)
        {
            transform.position = selected.transform.position;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}