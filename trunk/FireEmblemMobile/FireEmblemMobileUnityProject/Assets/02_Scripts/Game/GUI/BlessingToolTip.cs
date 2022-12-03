using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class BlessingToolTip : MonoBehaviour
    {
         
           public TextMeshProUGUI headerText;
           public TextMeshProUGUI descriptionText;
           
           public Image skillIcon;
           
           private Blessing blessing;
           private RectTransform rectTransform;
           public LayoutElement frame;
           public int characterWrapLimit;
           public LayoutElement layoutElement;
           private void Start()
           {
               rectTransform = GetComponent<RectTransform>();
           }

           // Update is called once per frame
           private void Update()
           {
               if (Application.isEditor)
               {
                   UpdateTextWrap(transform.position);
               }
           }
           
           void UpdateTextWrap(Vector3 position)
           {
       
               frame.enabled = false;
               frame.enabled = true;
               if(rectTransform==null)
                   rectTransform = GetComponent<RectTransform>();
               int headerLength = headerText.text.Length;
               int contentLength = descriptionText.text.Length;
               layoutElement.enabled =
                   (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;

        
               float pivotX = position.x / Screen.width;
               float pivotY = position.y / Screen.height;
               rectTransform.pivot = new Vector2(pivotX, pivotY);
           }
       
           public void ExitClicked()
           {
               gameObject.SetActive(false);
           }
           public void SetValues(Blessing blessing, string header, string description, Sprite icon, Vector3 position)
           {
               this.blessing = blessing;
               if (string.IsNullOrEmpty(header))
               {
                   headerText.gameObject.SetActive(false);
               }
               else
               {
                   headerText.gameObject.SetActive(true);
                   headerText.text = header;
               }

               descriptionText.text = description;
               skillIcon.sprite = icon;
               
               UpdateTextWrap(position);
       
           }
    }
}
