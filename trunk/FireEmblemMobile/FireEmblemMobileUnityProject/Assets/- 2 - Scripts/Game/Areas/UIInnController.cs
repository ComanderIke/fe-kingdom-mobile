using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInnController : MonoBehaviour
{
    // Start is called before the first frame update
    public Party party;
    public GameObject headline;
    public TextMeshProUGUI headlineText;
    public Image recruitCharacterImage;
    void Start()
    {
        
    }
    

    public void Show(Party instanceParty)
    {
        gameObject.SetActive(true);
        this.party = party;
        headline.SetActive(true);
        headlineText.SetText("Inn");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        headline.SetActive(false);
    }

    public void RestClicked()
    {
        
    }

    public void AcceptQuestClicked()
    {
        
    }

    public void RecruitCharacterClicked()
    {
        
    }
}
