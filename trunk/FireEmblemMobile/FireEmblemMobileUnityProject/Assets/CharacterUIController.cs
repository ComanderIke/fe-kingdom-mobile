using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUIController : MonoBehaviour
{
    public GameObject StatPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CharacterImageClicked()
    {
        StatPanel.gameObject.SetActive(!StatPanel.activeSelf);
    }
}
