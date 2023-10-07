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
        [SerializeField] private GameObject alreadyDonatedTextGO;
        [SerializeField] private CanvasGroup alreadyDonatedCanvasGroup;
        public void Show(Unit user, bool alreadyDonated)
        {
            alreadyDonatedTextGO.SetActive(alreadyDonated);
            alreadyDonatedCanvasGroup.enabled = alreadyDonated;
            alreadyDonatedCanvasGroup.blocksRaycasts = !alreadyDonated;
            
            gameObject.SetActive(true);
            Faith.SetText(""+user.Stats.BaseAttributes.FAITH);
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

        
    }
}
