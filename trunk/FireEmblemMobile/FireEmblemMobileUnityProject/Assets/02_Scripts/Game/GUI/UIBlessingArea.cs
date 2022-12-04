using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Game.GameActors.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class UIBlessingArea : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private TextMeshProUGUI Description;
        [SerializeField] private TextMeshProUGUI effect;
        [SerializeField] private TextMeshProUGUI Faith;
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private Button AcceptButton;
        [SerializeField] UIChurchController churchController;

        public void Show(Unit unit, Blessing blessing, bool alreadyAccepted)
        {
            gameObject.SetActive(true);
            Faith.SetText("" + unit.Stats.Attributes.FAITH);
            name.SetText(blessing.Name);
            Description.SetText(blessing.GetDurationDescription(unit.Stats.Attributes.FAITH));
            effect.SetText(blessing.Skill.Description);
            AcceptButton.interactable = !alreadyAccepted;
            icon.sprite = blessing.Skill.Icon;
            if (alreadyAccepted)
            {
                buttonText.text = "Received";
            }
            else
            {
                buttonText.text = "Accept";
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void AcceptClicked()
        {
            churchController.AcceptBlessing();
        }
    }
}