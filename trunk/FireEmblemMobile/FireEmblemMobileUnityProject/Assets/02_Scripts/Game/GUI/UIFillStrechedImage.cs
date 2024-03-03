using UnityEngine;

namespace Game.GUI
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
            rectTransform.offsetMax= new Vector2(-backGround.rect.width+(fill * backGround.rect.width), rectTransform.offsetMax.y);
        }
        void Update()
        {
           
        }
    }
}
