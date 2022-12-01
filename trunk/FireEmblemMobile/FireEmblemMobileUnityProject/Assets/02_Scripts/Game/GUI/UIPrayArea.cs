using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class UIPrayArea : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI Faith;
        [SerializeField] private Button smallDonateButton;
        [SerializeField] private Button mediumDonateButton;
        [SerializeField] private Button highDonateButton;
        [SerializeField] UIChurchController churchController;
        public void Show(Unit user)
        {
            gameObject.SetActive(true);
            Faith.SetText(""+user.Stats.Attributes.FAITH);
            smallDonateButton.interactable = true;
            mediumDonateButton.interactable = true;
            highDonateButton.interactable = true;
            if (!Player.Instance.Party.CanAfford(25))
            {
                smallDonateButton.interactable = false;
                mediumDonateButton.interactable = false;
                highDonateButton.interactable = false;
            }
            else if (!Player.Instance.Party.CanAfford(50))
            {
                mediumDonateButton.interactable = false;
                highDonateButton.interactable = false;
            }
            if (!Player.Instance.Party.CanAfford(100))
            {
                highDonateButton.interactable = false;
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void DonateSmallClicked()
        {
            churchController.DonateSmall();
        }
        public void DonateMediumClicked()
        {
            churchController.DonateMedium();
        }
        public void DonateHighClicked()
        {
            churchController.DonateHigh();
        }
    }
}
