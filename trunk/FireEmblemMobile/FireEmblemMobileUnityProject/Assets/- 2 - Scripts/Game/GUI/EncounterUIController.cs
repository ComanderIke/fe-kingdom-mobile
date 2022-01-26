using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class EncounterUIController : MonoBehaviour
{
    public TextMeshProUGUI Gold;
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
}
