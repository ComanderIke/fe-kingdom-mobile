using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class UIBlessingArea : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private TextMeshProUGUI Description;
        [SerializeField] private TextMeshProUGUI effect;
        [SerializeField] private TextMeshProUGUI Faith;
        [SerializeField] private Button AcceptButton;
        [SerializeField] UIChurchController churchController;

        public void Show(Unit unit, Blessing blessing, bool alreadyAccepted)
        {
            gameObject.SetActive(true);
            Faith.SetText("" + unit.Stats.Attributes.FAITH);
            name.SetText(blessing.name);
            Description.SetText(blessing.Description);
            effect.SetText(blessing.effectDescription);
            AcceptButton.interactable = !alreadyAccepted;
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