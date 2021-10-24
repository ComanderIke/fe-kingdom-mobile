using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class ShadowController : MonoBehaviour
{
    public Color shadowColor;

    public Vector3 shadowOffset;
    public Vector3 shadowDirection;
    public Vector3 shadowScale=Vector3.one;
    private SpriteRenderer shadowCaster;

    private Animator animator;
    private GameObject shadowGo;
    // Start is called before the first frame update
    void OnEnable()
    {
        InitShadow();
    }

    void InitShadow()
    {
        shadowCaster = GetComponent<SpriteRenderer>();
        if(shadowGo != null)
            DestroyImmediate(shadowGo);
        if(shadowGo==null)
            if (GetComponentInChildren<SpriteRenderer>().gameObject.name == "shadow")
                shadowGo = GetComponentInChildren<SpriteRenderer>().gameObject;
        if (shadowGo == null)
        {
            shadowGo = GameObject.Instantiate(new GameObject(), transform, false);
            shadowGo.name = "shadow";

        }
        shadowGo.GetComponent<SpriteRenderer>().sprite = shadowCaster.sprite;
        shadowGo.GetComponent<SpriteRenderer>().color = shadowColor;
        shadowGo.transform.localPosition = shadowOffset;
        shadowGo.transform.localRotation = Quaternion.Euler(shadowDirection);
        shadowGo.transform.localScale = shadowScale;
        shadowGo.layer = gameObject.layer;
        shadowGo.GetComponent<SpriteRenderer>().sortingLayerID = shadowCaster.sortingLayerID;
        shadowGo.GetComponent<SpriteRenderer>().sortingOrder = shadowCaster.sortingOrder;
        shadowGo.GetComponent<SpriteRenderer>().flipX = shadowCaster.flipX;;
       
    }

    void UpdateShadow()
    {
        shadowGo.GetComponent<SpriteRenderer>().sprite = shadowCaster.sprite;
        shadowGo.GetComponent<SpriteRenderer>().flipX = shadowCaster.flipX;;
    }

    private void OnDisable()
    {
        DestroyImmediate(shadowGo);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateShadow();
    }
}
