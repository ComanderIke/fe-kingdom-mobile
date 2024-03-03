using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class UIPageCountPointController : MonoBehaviour
    {
        [SerializeField] private Image colorImage;
        [SerializeField] private Sprite selected;
        [SerializeField] private Sprite normal;
        public void SetColor(Color color)
        {
            colorImage.color = color;
        }

        public void Select()
        {
            colorImage.sprite = selected;
            //throw new System.NotImplementedException();
        }

        public void Deselect()
        {
            colorImage.sprite = normal;
            // throw new System.NotImplementedException();
        }
    }
   
}