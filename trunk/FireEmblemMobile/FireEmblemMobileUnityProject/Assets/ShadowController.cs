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
    public Material shadowMat;
    private Animator animator;
    private GameObject shadowGo;
    // Start is called before the first frame update
    void OnEnable()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "Shadow")
                shadowGo=child.gameObject;
        }
        shadowCaster = GetComponent<SpriteRenderer>();
        if(shadowGo==null)
            CreateShadow();
        else 
            InitShadow();
    }

    void CreateShadow()
    {
        shadowGo = GameObject.Instantiate(new GameObject(), transform, false);
        shadowGo.name = "Shadow";
        shadowGo.AddComponent<SpriteRenderer>();
        shadowGo.tag = "Shadow";
        
        InitShadow();
    }
    void InitShadow()
    {
        shadowGo.GetComponent<SpriteRenderer>().sprite = shadowCaster.sprite;
        shadowGo.GetComponent<SpriteRenderer>().color = shadowColor;
        shadowGo.transform.localPosition = shadowOffset;
        shadowGo.transform.localRotation = Quaternion.Euler(shadowDirection);
        shadowGo.transform.localScale = shadowScale;
        shadowGo.layer = gameObject.layer;
        shadowGo.GetComponent<SpriteRenderer>().sortingLayerID = shadowCaster.sortingLayerID;
        shadowGo.GetComponent<SpriteRenderer>().sortingOrder = shadowCaster.sortingOrder;
        shadowGo.GetComponent<SpriteRenderer>().flipX = shadowCaster.flipX;;
        if (shadowMat != null)
            shadowGo.GetComponent<SpriteRenderer>().material = shadowMat;

    }

    void UpdateShadow()
    {
        shadowGo.GetComponent<SpriteRenderer>().sprite = shadowCaster.sprite;
        shadowGo.GetComponent<SpriteRenderer>().flipX = shadowCaster.flipX;;
    }
    

    // Update is called once per frame
    void Update()
    {
        UpdateShadow();
    }
}
