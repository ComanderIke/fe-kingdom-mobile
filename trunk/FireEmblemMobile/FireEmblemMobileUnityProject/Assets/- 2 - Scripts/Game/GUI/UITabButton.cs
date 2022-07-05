using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UITabButton : MonoBehaviour
{
    public UITabGroup tabGroup;

    public Image backGround;
    // Start is called before the first frame update
    void Start()
    {
        backGround = GetComponent<Image>();
        tabGroup.Subscribe(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void Clicked()
    {
        tabGroup.OnTabSelected(this);
    }
    
}
