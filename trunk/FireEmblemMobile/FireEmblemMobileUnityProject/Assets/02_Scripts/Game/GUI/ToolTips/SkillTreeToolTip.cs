using Game.GUI.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.ToolTips
{
    public class SkillTreeToolTip : MonoBehaviour
    {
        public TextMeshProUGUI headerText;
        public TextMeshProUGUI descriptionText;

   

        public Image skillIcon;

        public Button learnButton;
        public TextMeshProUGUI useButtonText;
        private SkillTreeEntryUI skillTreeEntry;
        private RectTransform rectTransform;

        // Start is called before the first frame update
        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (Application.isEditor)
            {
                UpdateTextWrap(transform.position);
            }
        }

        public void LearnClicked()
        {
            Debug.Log("LearnSkillClicked");
            skillTreeEntry.LearnClicked();
            gameObject.SetActive(false);
        }
        void UpdateTextWrap(Vector3 position)
        {

            transform.position = position+ new Vector3(0,100,0);
        }

        public void ExitClicked()
        {
            gameObject.SetActive(false);
        }
        public void SetValues(SkillTreeEntryUI skillTreeEntry, string header, string description, Sprite icon, Vector3 position)
        {
            this.skillTreeEntry = skillTreeEntry;
            if (string.IsNullOrEmpty(header))
            {
                headerText.gameObject.SetActive(false);
            }
            else
            {
                headerText.gameObject.SetActive(true);
                headerText.text = header;
            }
        
            useButtonText.text = "Learn";
       
            descriptionText.text = description;
            skillIcon.sprite = icon;
        
            UpdateTextWrap(position);

        }
    }
}