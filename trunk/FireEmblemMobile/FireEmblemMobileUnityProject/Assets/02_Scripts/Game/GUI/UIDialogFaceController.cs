using Game.GameActors.Units;
using Game.GameActors.Units.Visuals;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class UIDialogFaceController : MonoBehaviour
    {
        [SerializeField] private DialogSpriteSet characterSpriteSet;
        [SerializeField] private Image face;
        [SerializeField] private Image mouth;
        [SerializeField] private Image eyes;
        void Start()
        {
            UpdateUI();
        }

        void UpdateUI()
        {
            face.sprite = characterSpriteSet.FaceSprite;
            mouth.gameObject.SetActive(characterSpriteSet.MouthClosed!=null);
            mouth.sprite = characterSpriteSet.MouthClosed;
            mouth.gameObject.SetActive(characterSpriteSet.EyesOpen!=null);
            eyes.sprite = characterSpriteSet.EyesOpen;
        }

        public void Init(DialogSpriteSet dialogSpriteSet)
        {
            characterSpriteSet = dialogSpriteSet;
            UpdateUI();

        }
        public void EyesOpen()
        {
            // Debug.Log("EyesOpen");
            eyes.sprite = characterSpriteSet.EyesOpen;
        }
        public void EyesClosed()
        {
            // Debug.Log("EyesClosed");
            eyes.sprite = characterSpriteSet.EyesClosed;
        }

        public void MouthOpen()
        {
            // Debug.Log("MouthOpen");
            mouth.sprite = characterSpriteSet.MouthOpen;
        }
        public void MouthClosed()
        {
            // Debug.Log("MouthClosed");
            mouth.sprite = characterSpriteSet.MouthClosed;
        }
    }
}
