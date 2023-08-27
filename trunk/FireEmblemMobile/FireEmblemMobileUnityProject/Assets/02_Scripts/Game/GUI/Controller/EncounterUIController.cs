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
    [FormerlySerializedAs("Gold")] public UIRessourceAmount goldAmount;
    public UIRessourceAmount graceAmount;
    public UIInnController UIInnController;
    public UISmithyController UISmithyController;
    public UIChurchController UIChurchController;
    public UIEventController UIEventController;
    public UIMerchantController UIMerchantController;

    public UIMoralityBar MoralityBar;
    private Party party;
    // Start is called before the first frame update
    public void Init(Party party)
    {
        this.party = party;
        party.onGoldChanged += GoldChanged;
        party.onGraceChanged += GraceChanged;
        GoldChanged(party.Money);
        GraceChanged(party.CollectedGrace);

        MoralityBar.Show(party.Morality);
    }

    private void OnDestroy()
    {
        if (party != null)
        {
            party.onGoldChanged -= GoldChanged;
            party.onGraceChanged -= GraceChanged;
        }
    }

    void GoldChanged(int gold)
    {
        goldAmount.Amount = gold;
    }
    void GraceChanged(int grace)
    {
        graceAmount.Amount = grace;
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
