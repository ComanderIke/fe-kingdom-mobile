using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace LostGrace
{
    public class SelectFileUI : MonoBehaviour
    {
        [SerializeField] private Transform layout;
        [SerializeField] private SaveFileUI slot1Button;
        [SerializeField] private SaveFileUI slot2Button;
        [SerializeField] private SaveFileUI slot3Button;
        [SerializeField] private TextFieldPopUp textFieldPopUp;
        
        public void Show()
        {
            Debug.Log("CheckLoadFiles");
            gameObject.SetActive(true);
            if (SaveGameManager.FileSlotExists(1))
            {
                Debug.Log("Slot 1 Exists"); 
                slot1Button.SetLoaded(true);
                slot1Button.UpdateText(SaveGameManager.GetFileSlotName(1));
            }
            else
            {
                slot1Button.SetLoaded(false);
                slot1Button.UpdateText("New Game");
            }
            if (SaveGameManager.FileSlotExists(2))
            {
                Debug.Log("Slot 2 Exists");
                slot2Button.SetLoaded(true);
                slot2Button.UpdateText(SaveGameManager.GetFileSlotName(2));
            }
            else
            {
                slot2Button.SetLoaded(false);
                slot2Button.UpdateText("New Game");
            }
            if (SaveGameManager.FileSlotExists(3))
            {
                Debug.Log("Slot 3 Exists");
                slot3Button.SetLoaded(true);
                slot3Button.UpdateText(SaveGameManager.GetFileSlotName(3));
            }
            else
            {
                slot3Button.SetLoaded(false);
                slot3Button.UpdateText("New Game");
            }
            slot1Button.SetInteractable(true);
            slot2Button.SetInteractable(true);
            slot3Button.SetInteractable(true);
            
            // layout.DeleteAllChildren();
            // for (int i = 0; i < SaveGameManager.SaveFileCount; i++)
            // {
            //     var go=Instantiate(loadFilePrefab, layout);
            // }
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private int selected = -1;
        public void SaveFileClicked(int slot)
        {
            selected = slot;
            textFieldPopUp.onSubmittedText -= FileNameSubmitted;
            textFieldPopUp.onSubmittedText += FileNameSubmitted;
            textFieldPopUp.Show("File "+slot);
            slot1Button.SetInteractable(false);
            slot2Button.SetInteractable(false);
            slot3Button.SetInteractable(false);
        }

        void FileNameSubmitted(string name)
        {
            textFieldPopUp.onSubmittedText -= FileNameSubmitted;
            //DO some Animation SaveFile changes Color and text
            //All Buttons non Interactable

            StartCoroutine(AnimationCoroutine(name));
        }

        IEnumerator AnimationCoroutine(string name)
        {
            switch (selected)
            {
                case 1:  slot1Button.SetLoaded(true);
                    break;
                case 2:  slot2Button.SetLoaded(true);break;
                case 3:  slot3Button.SetLoaded(true);break;
            }
            yield return new WaitForSeconds(.5f);
            switch (selected)
            {
                case 1:  slot1Button.UpdateText(name);
                    SaveGameManager.NewGame(1, name);
                    break;
                case 2:  slot2Button.UpdateText(name);
                    SaveGameManager.NewGame(2, name);break;
                case 3:  slot3Button.UpdateText(name);
                    SaveGameManager.NewGame(3, name);break;
            }
            yield return new WaitForSeconds(1.0f);
            Hide();
        }
    }
}
