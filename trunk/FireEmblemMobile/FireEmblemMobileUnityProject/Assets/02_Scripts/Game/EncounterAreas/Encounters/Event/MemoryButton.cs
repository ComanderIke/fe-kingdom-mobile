using Game.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Game.EncounterAreas.Encounters.Event
{
    public class MemoryButton : MonoBehaviour
    {
        public Sprite SecretSprite;

        public Image image;

        public Sprite itemSprite;
        public Object userData;

        public MemoryMiniGame MemoryController;

        public Button button;


        public bool revealed = false;
    

        void TurnAnimation()
        {
            if (!revealed)
            {
                revealed = true;
                MemoryController.RevealField(this);
                LeanTween.rotateY(gameObject, 90f, .4f).setEaseInQuad().setOnComplete(() =>
                {
                    image.sprite = itemSprite;
                    LeanTween.rotateY(gameObject, 180f, .4f).setEaseOutBounce();
                    if (MemoryController.TurnBack(this))
                    {
                        MonoUtility.DelayFunction(TurnBack,1.0f);
                    }
                });
           
            }
            else
            {
                TurnBack();
            }
        }

        public void OnCllick()
        {
            if(MemoryController.CanTurnField())
                TurnAnimation();
        }

        public void TurnBack()
        {
            revealed = false;
            LeanTween.rotateY(gameObject, 90f, .4f).setEaseInQuad().setOnComplete(() =>
            {
                image.sprite = SecretSprite;
                LeanTween.rotateY(gameObject, 0f, .4f).setEaseOutQuad();
            });
        }

        public void SetInActive()
        {
            button.interactable=false;
        }
        public void SetActive()
        {
            button.interactable=true;
        }
    }
}