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
            Debug.Log("ON ENABLE SET SIZE DELTA");
            GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
