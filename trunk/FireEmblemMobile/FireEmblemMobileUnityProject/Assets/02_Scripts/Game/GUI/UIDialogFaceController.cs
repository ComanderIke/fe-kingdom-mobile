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
            mouth.sprite = characterSpriteSet.MouthClosed;
            eyes.sprite = characterSpriteSet.EyesOpen;
        }

        public void Init(Unit unit)
        {
            characterSpriteSet = unit.visuals.CharacterSpriteSet.DialogSpriteSet;
            
        }
        public void EyesOpen()
        {
            Debug.Log("EyesOpen");
            eyes.sprite = characterSpriteSet.EyesOpen;
        }
        public void EyesClosed()
        {
            Debug.Log("EyesClosed");
            eyes.sprite = characterSpriteSet.EyesClosed;
        }

        public void MouthOpen()
        {
            Debug.Log("MouthOpen");
            mouth.sprite = characterSpriteSet.MouthOpen;
        }
        public void MouthClosed()
        {
            Debug.Log("MouthClosed");
            mouth.sprite = characterSpriteSet.MouthClosed;
        }
    }
}
