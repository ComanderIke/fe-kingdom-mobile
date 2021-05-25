using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpriteOutLinePressed : MonoBehaviour
{
    public Material normal;

    public Material selected;

    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void OnMouseEnter()
    // {
    //     
    //     spriteRenderer.material = selected;
    // }
    // public void OnMouseExit()
    // {
    //     
    //     spriteRenderer.material = normal;
    // }
    public void OnMouseDown()
    {
      
        spriteRenderer.material = selected;
    }
    public void OnMouseUp()
    {
  
        spriteRenderer.material = normal;
    }
}
