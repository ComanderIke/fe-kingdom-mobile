using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class EncounterUIController : MonoBehaviour
{
    public TextMeshProUGUI Gold;

    public UIInnController UIInnController;
    public UISmithyController UISmithyController;
    public UIChurchController UIChurchController;
    public UIEventController UIEventController;
    public UIMerchantController UIMerchantController;
    // Start is called before the first frame update
    public void Init(Party party)
    {
        party.onGoldChanged += GoldChanged;
        GoldChanged(party.Money);
    }

    void GoldChanged(int gold)
    {
        Gold.SetText(""+gold);
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
