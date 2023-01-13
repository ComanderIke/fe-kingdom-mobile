using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class BlessingToolTip : MonoBehaviour
    {
         
           public TextMeshProUGUI headerText;
           public TextMeshProUGUI descriptionText;
           public TextMeshProUGUI durationDescription;
           public GameObject extraEffectPrefab;
           public Transform extraEffectParent;
           public Image skillIcon;
           [SerializeField]
           private RectTransform rectTransform;
           public LayoutElement frame;
           public int characterWrapLimit;
           public LayoutElement layoutElement;

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
           public void SetValues(CurseBlessBase blessing, string header, string description,Sprite icon, Vector3 position)
           {
               if (string.IsNullOrEmpty(header))
               {
                   headerText.gameObject.SetActive(false);
               }
               else
               {
                   headerText.gameObject.SetActive(true);
                   headerText.text = header;
               }

               durationDescription.text = blessing.GetShortDurationDescription();
               descriptionText.text = description;
               skillIcon.sprite = icon;
               var effects = blessing.Skill.GetEffectDescription();
               if (effects != null)
               {
                   extraEffectParent.gameObject.SetActive(true);
                   extraEffectParent.DeleteAllChildren();
                   foreach (var effect in effects)
                   {
                       var go = Instantiate(extraEffectPrefab, extraEffectParent);
                       go.GetComponent<TextMeshProUGUI>().text = effect.label;
                       go = Instantiate(extraEffectPrefab, extraEffectParent);
                       go.GetComponent<TextMeshProUGUI>().text = effect.value;
                       
                   }
               }
               else
               {
                   extraEffectParent.gameObject.SetActive(false);
                   extraEffectParent.DeleteAllChildren();
               }
               rectTransform.anchoredPosition = position+ new Vector3(0,200,0);
               UpdateTextWrap(position);
       
           }
    }
}
