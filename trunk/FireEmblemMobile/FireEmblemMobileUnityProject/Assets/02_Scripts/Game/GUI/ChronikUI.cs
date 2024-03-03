using System.Collections;
using System.Collections.Generic;
using Game.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public interface IChronikEntry
    {
        Sprite BodySprite { get; set; }
        Sprite FaceSprite { get; set; }
        Sprite AlternateBodySprite { get; set; }
        Sprite AlternateFaceSprite { get; set; }

        string Name { get; set; }
        string Description { get; set; }
    }

    public class ChronikUI : UIMenu
    {
        
        [SerializeField]private List<IChronikEntry> entries;
        private IChronikEntry currentEntry;
        private int currentIndex;
        [SerializeField] private UIChronikEntryList entryListUI;
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Image middleImage;
        [SerializeField] private Image middleImageFace;
        [SerializeField] private GameObject alternateButton;
        private bool showBody = true;
        private bool showAlternate = false;
        [SerializeField] private TextMeshProUGUI toggleButtonText;
        
        public void Show(List<IChronikEntry> entries)
        {
            showBody = true;
            showAlternate = false;
            this.entries = entries;
            entryListUI.Init(entries);
            entryListUI.OnSelectEntry -= SelectEntry;
            entryListUI.OnSelectEntry += SelectEntry;
            StartCoroutine(ShowCoroutine());
            SetActiveEntry(0);
        }

        void SelectEntry(IChronikEntry entry)
        {
            SetActiveEntry(entry);
        }

        public void ToogleImageClicked()
        {
            showBody = !showBody;
            
            UpdateUI();
        }
        public void ToogleAlternateClicked()
        {
            showAlternate = !showAlternate;
            
            UpdateUI();
        }
        void SetActiveEntry(IChronikEntry entry)
        {
            currentIndex = entries.IndexOf(entry);
            currentEntry = entries[currentIndex];
            UpdateUI();
        }
        void SetActiveEntry(int index)
        {
            currentIndex = 0;
            currentEntry = entries[currentIndex];
            UpdateUI();
        }

        void UpdateUI()
        {
            toggleButtonText.text = showBody ? "Show Portrait" : "Show Body";
            bool showAltButton = false;
            if (currentEntry.AlternateBodySprite==null)
            {
                showAlternate = false;
            }
            else
            {
                showAltButton = true;
            }
            alternateButton.gameObject.SetActive(showAltButton);
            name.text = "<shine>"+currentEntry.Name+"</shine>";
            description.text = "{fade}"+currentEntry.Description;
            middleImage.enabled = showBody;
            middleImageFace.enabled = !showBody;
            middleImage.sprite = showAlternate?(currentEntry.AlternateBodySprite==null?currentEntry.BodySprite:currentEntry.AlternateBodySprite):currentEntry.BodySprite;
            middleImageFace.sprite=showAlternate?(currentEntry.AlternateFaceSprite==null?currentEntry.FaceSprite:currentEntry.AlternateFaceSprite):currentEntry.FaceSprite;
            entryListUI.UpdateUI(currentIndex);
        }
        IEnumerator ShowCoroutine()
        {
            base.Show();
            
            yield return null;
        }

        public override void BackClicked()
        {
            Hide();
            
        }
        IEnumerator HideCoroutine()
        {
            // TweenUtility.FadeOut(newGameButtonCanvasGroup);
            base.Hide();
      
            yield return null;
        }



        public override void Hide()
        {
            entryListUI.OnSelectEntry -= SelectEntry;
            StartCoroutine(HideCoroutine());
        }
    }
}
