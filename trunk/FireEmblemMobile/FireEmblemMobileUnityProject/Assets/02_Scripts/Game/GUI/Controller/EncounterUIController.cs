using System;
using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Serialization;

public class EncounterUIController : MonoBehaviour
{
    [FormerlySerializedAs("Gold")] public UIRessourceAmount ressourceAmount;
    public TextMeshProUGUI SmithingStones;

    public UIInnController UIInnController;
    public UISmithyController UISmithyController;
    public UIChurchController UIChurchController;
    public UIEventController UIEventController;
    public UIMerchantController UIMerchantController;

    private Party party;
    // Start is called before the first frame update
    public void Init(Party party)
    {
        this.party = party;
        party.onGoldChanged += GoldChanged;
        GoldChanged(party.Money);

    }

    private void OnDestroy()
    {
        if (party != null)
        {
            party.onGoldChanged -= GoldChanged;
        }
    }

    void GoldChanged(int gold)
    {
        ressourceAmount.Amount = gold;
    }

    public void UpdateUIScreens()
    {
        if (UIInnController.canvas.enabled)
        {
            UIInnController.UpdateUI();
        }
        if (UISmithyController.canvas.enabled)
        {
            UISmithyController.UpdateUI();
        }
        if (UIMerchantController.canvas.enabled)
        {
            UIMerchantController.UpdateUI();
        }
        if (UIChurchController.canvas.enabled)
        {
            UIChurchController.UpdateUI();
        }
        if (UIEventController.canvas.enabled)
        {
            UIEventController.UpdateUI();
        }
   
    }
}
