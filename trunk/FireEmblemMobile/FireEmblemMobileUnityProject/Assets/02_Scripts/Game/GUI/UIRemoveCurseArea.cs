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
    public class UIRemoveCurseArea : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI Description;
        [SerializeField] private TextMeshProUGUI effect;
        [SerializeField] private TextMeshProUGUI Faith;
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private Button AcceptButton;
        [SerializeField] UIChurchController churchController;
        [SerializeField]GameObject extraEffectDescriptionPrefab;
        [SerializeField]GameObject extraEffectValuePrefab;
        [SerializeField]GameObject extraEffectContainerPrefab;
        [SerializeField] Transform extraEffectParent;

        public void Show(Unit unit,  bool alreadyRemovedCurse)
        {
            gameObject.SetActive(true);
            Faith.SetText("" + unit.Stats.BaseAttributes.FAITH);
            // nameText.SetText(blessing.Name);
            // Description.SetText(blessing.Description);
            // effect.SetText(blessing.Description);
            // AcceptButton.interactable = !alreadyAccepted;
            // icon.sprite = blessing.Icon;
            // if (alreadyAccepted)
            // {
            //     buttonText.text = "Received";
            // }
            // else
            // {
            //     buttonText.text = "Accept";
            // }
            // var effects = blessing.GetEffectDescription();
            // if (effects != null)
            // {
            //     extraEffectParent.gameObject.SetActive(true);
            //     extraEffectParent.DeleteAllChildren();
            //     foreach (var effect in effects)
            //     {
            //         var container = Instantiate(extraEffectContainerPrefab, extraEffectParent);
            //         var label = Instantiate(extraEffectDescriptionPrefab, container.transform);
            //         var value = Instantiate(extraEffectValuePrefab, container.transform);
            //         label.GetComponent<TextMeshProUGUI>().text = effect.label;
            //         value.GetComponent<TextMeshProUGUI>().text = effect.value;
            //            
            //     }
            // }
            // else
            // {
            //     extraEffectParent.gameObject.SetActive(false);
            //     extraEffectParent.DeleteAllChildren();
            // }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        // public void AcceptClicked()
        // {
        //     churchController.RemoveCurse();
        // }
    }
}