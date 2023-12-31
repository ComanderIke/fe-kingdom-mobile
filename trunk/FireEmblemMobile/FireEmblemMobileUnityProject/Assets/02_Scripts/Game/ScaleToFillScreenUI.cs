using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class ScaleToFillScreenUI : MonoBehaviour
    {
        // Start is called before the first frame update
        void OnEnable()
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
