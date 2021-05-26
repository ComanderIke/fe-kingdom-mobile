using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpriteOutLinePressed : MonoBehaviour
{
    public Material normal;

    public Material selected;

    public GameObject activate;
    public SpriteRenderer spriteRenderer;

    public static Action<GameObject> OnClicked;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void OnMouseDown()
    {
      
        spriteRenderer.material = selected;
        
    }


    public void OnMouseOver()
    {
        
    }

    public void OnMouseUp()
    {
  
        spriteRenderer.material = normal;
    }
    public void OnMouseUpAsButton()
    {
        if(activate!=null)
            activate.SetActive(true);
        OnClicked?.Invoke(this.gameObject);
    }
}
