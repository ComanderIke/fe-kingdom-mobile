using UnityEngine;
using UnityEngine.UI;

namespace Game.Utility.UI
{
    public class MenuVisibilityCtrl : MonoBehaviour {
        [SerializeField] bool shouldStartVisible;
 
        GameObject _myGameObj;
        GameObject _invisibleBG;
 
        void Awake ()
        {
            setupInvisibleBG ();
            _myGameObj = gameObject;
            if (!shouldStartVisible)
                hide();
        }
 
        void setupInvisibleBG ()
        {
            _invisibleBG = new GameObject("InvisibleBG");
 
            InvisibleBGCtrl tempInvisibleBGCtrl = _invisibleBG.AddComponent<InvisibleBGCtrl>();
            tempInvisibleBGCtrl.setParentCtrl (this);
 
            Image tempImage = _invisibleBG.AddComponent<Image>();
            tempImage.color = new Color(1f, 1f, 1f, 0f);
 
            RectTransform tempTransform = _invisibleBG.GetComponent<RectTransform>();
            tempTransform.anchorMin = new Vector2(0f, 0f);
            tempTransform.anchorMax = new Vector2(1f, 1f);
            tempTransform.offsetMin = new Vector2(0f, 0f);
            tempTransform.offsetMax = new Vector2(0f, 0f);
            tempTransform.SetParent(GetComponentsInParent<Transform>()[1], false);
            tempTransform.SetSiblingIndex(transform.GetSiblingIndex()); // put it right beind this panel in the hierarchy
        }
 
        void OnEnable ()
        {
            _invisibleBG.SetActive(true);
        }
 
        public void hide ()
        {
            _myGameObj.SetActive(false);
            _invisibleBG.SetActive(false);
        }
    }
}