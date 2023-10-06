using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class UIFillStrechedImage : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private RectTransform backGround;
        void Start()
        {
        
        }

        public void SetFill(float fill)
        {
            Debug.Log("SetFill");
            rectTransform.offsetMax= new Vector2(-backGround.rect.width+(fill * backGround.rect.width), rectTransform.offsetMax.y);
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("A Pressed!");
                SetFill(0f);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                SetFill(.5f);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                SetFill(1f);
            }
        }
    }
}
