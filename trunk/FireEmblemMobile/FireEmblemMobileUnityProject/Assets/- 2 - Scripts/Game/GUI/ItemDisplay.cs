using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ItemDisplay : MonoBehaviour
{
    private Image image;

    public Item item;
    // Start is called before the first frame update
    void OnEnable()
    {
        image = GetComponent<Image>();
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        if(item!=null)
            image.sprite = item.Sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
