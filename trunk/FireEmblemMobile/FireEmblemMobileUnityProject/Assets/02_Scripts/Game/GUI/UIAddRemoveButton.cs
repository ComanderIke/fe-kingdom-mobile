using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class UIAddRemoveButton : MonoBehaviour
    {
        private bool add = true;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI addText;
        [SerializeField] private TextMeshProUGUI removeText;
        [SerializeField] private Sprite addSprite;
        [SerializeField] private Sprite removeSprite;
        [SerializeField] private int smallFontSize;
        [SerializeField] private int bigFontSize;
        [SerializeField] private float fontAlphaInactive;
       
        private void Switch()
        {
           
            image.sprite = add ? addSprite : removeSprite;
            addText.fontSize = add ? bigFontSize : smallFontSize;
            removeText.fontSize = add ?  smallFontSize: bigFontSize;
            addText.color = new Color(addText.color.r, addText.color.g, addText.color.b, add ? 1 : fontAlphaInactive);
            removeText.color = new Color(removeText.color.r, removeText.color.g, removeText.color.b, add ? fontAlphaInactive :1 );

        }

        public void ShowRemove()
        {
            add = false;
            Switch();
            
        }
        public void ShowAdd()
        {
            add = true;
            Switch();
            
        }
    }
}
