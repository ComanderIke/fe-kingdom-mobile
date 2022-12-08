using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using __2___Scripts.Game.Utility;
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
        [SerializeField]GameObject extraEffectPrefab;
        [SerializeField] Transform extraEffectParent;

        public void Show(Unit unit, Blessing blessing, bool alreadyAccepted)
        {
            gameObject.SetActive(true);
            Faith.SetText("" + unit.Stats.BaseAttributes.FAITH);
            name.SetText(blessing.Name);
            Description.SetText(blessing.GetDurationDescription(unit.Stats.BaseAttributes.FAITH));
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
            var effects = blessing.Skill.GetEffectDescription();
            if (effects != null)
            {
                extraEffectParent.gameObject.SetActive(true);
                extraEffectParent.DeleteAllChildren();
                foreach (var effect in effects)
                {
                    var go = Instantiate(extraEffectPrefab, extraEffectParent);
                    go.GetComponent<TextMeshProUGUI>().text = effect.label;
                    go = Instantiate(extraEffectPrefab, extraEffectParent);
                    go.GetComponent<TextMeshProUGUI>().text = effect.value;
                       
                }
            }
            else
            {
                extraEffectParent.gameObject.SetActive(false);
                extraEffectParent.DeleteAllChildren();
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