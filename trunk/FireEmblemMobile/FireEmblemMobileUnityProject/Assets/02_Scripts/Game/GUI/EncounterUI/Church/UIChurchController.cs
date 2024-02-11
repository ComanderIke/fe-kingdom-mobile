using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using LostGrace;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChurchController : MonoBehaviour
{
   
    public Canvas canvas;
    public ChurchEncounterNode node;
    [HideInInspector]
    public Party party;

    [SerializeField] private ShrineController shrineController;
    [SerializeField] private GameObject characterFacesContainers;
    [SerializeField] private GameObject characterFacePrefab;
    [SerializeField] private TextMeshProUGUI godStatueNameText;
    [SerializeField] private TextMeshProUGUI Bonusestext;
    [SerializeField] private Image godStatueImage;
    [SerializeField] private GameObject godsContainers;
    [SerializeField] private GameObject godTabPrefab;
    [SerializeField] private UIUnitIdleAnimation unitIdleAnimation;
    [SerializeField] private OKCancelDialogController okCancelDialogController;
    private Church church;
 
    [SerializeField] private Button curseButton;
    [SerializeField] private TextMeshProUGUI faithStat;
    [SerializeField] private ChampionUI championUI;

    [SerializeField] private List<God> gods;
    private List<UIGodBlessing> uiGodBlessings;
    private int selectedGod = 0;
    private int prevGod = 0;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color tooExpensiveTextColor;
    [SerializeField] private Button prayButton;
    [SerializeField] private Button donateButton;
    [SerializeField] private Button receiveBlessingButton;
    [SerializeField] private float goldExpConvert = .5f;
    [SerializeField] private float faithExpMult = 2f;
    [SerializeField] private List<Unit> alreadyPrayed;
    [SerializeField] private TextMeshProUGUI donateGoldCost;
    [SerializeField] private TextMeshProUGUI bondExpText;
    [SerializeField] private TextMeshProUGUI bondLevelText;
    [SerializeField] private MMProgressBar bondExpBar;
    private int goldCost = 100;
    private int donateExtraExp = 100;
    public void UpdateUI()
    {

        faithStat.text = ""+party.ActiveUnit.Stats.CombinedAttributes().FAITH;
        uiGodBlessings = new List<UIGodBlessing>();
        int cnt = 0;
        bool affordable = party.CanAfford(goldCost);
       
        donateButton.interactable = affordable;
        donateButton.GetComponentInChildren<TextMeshProUGUI>().text = donateButton.interactable ? "<bounce>Donate" : "</bounce>Donate";
        donateGoldCost.text = ""+goldCost;
        donateGoldCost.color = affordable?normalColor:tooExpensiveTextColor;
        receiveBlessingButton.gameObject.SetActive(false);
        godStatueNameText.text =  gods[selectedGod].Name;
        godStatueImage.sprite =  gods[selectedGod].StatueSprite;
        Bonusestext.text = church.GetGod().Name + "\nBond Exp +15%";
        bondExpText.text = party.ActiveUnit.Bonds.GetBondExperience(gods[selectedGod]) + "/100";
        bondLevelText.text = "Lv. " + party.ActiveUnit.Bonds.GetBondLevel(gods[selectedGod]);
        bondExpBar.UpdateBar01(party.ActiveUnit.Bonds.GetBondExperience(gods[selectedGod])/100f);
        if (party.CanReceiveBlessing(party.ActiveUnit, gods[selectedGod]))
        {
            receiveBlessingButton.gameObject.SetActive(true);
        }
        championUI.Show(GetUnitFromGod(gods[selectedGod]), party.ActiveUnit.Bonds.GetBondLevel(gods[selectedGod])>1);
        foreach (var god in gods)
        {
            // var parent = cnt>=4?bottomRowLayout:topRowParentLayout;
            // var go = Instantiate(GodBlessingUIPrefab, parent);
            // var uiGodController= go.GetComponent<UIGodBlessing>();
            // Unit blessedUnit = GetUnitFromGod(god);
            // uiGodController.Show(party.ActiveUnit,god, cnt, GoldToExp((int)slider.value, party.ActiveUnit.Stats.CombinedAttributes().FAITH), selectedGod == cnt, blessedUnit);
            // uiGodController.onClicked += GodClicked;
            // if(cnt==selectedGod)
            //     uiGodController.Select();
            // uiGodBlessings.Add(uiGodController);
            // cnt++;
        }
        unitIdleAnimation.Show(party.ActiveUnit);
        if (prevGod != selectedGod)
        {
            if (Math.Abs(prevGod - selectedGod) > 1)
            {
                shrineController.JumpToGod(selectedGod);
            }
           else if(prevGod>selectedGod)
                shrineController.NextStatue();
           else
           {
               shrineController.PrevStatue();
           }
            //unitIdleAnimation.PlayRunning(selectedGod<prevGod);
            prevGod = selectedGod;
        }
        shrineController.SetUnit(party.ActiveUnit);

        curseButton.gameObject.SetActive(party.ActiveUnit.Curses.Count != 0);
        prayButton.GetComponentInChildren<TextMeshProUGUI>().text=(!alreadyPrayed.Contains(party.ActiveUnit)?"<bounce>Pray" : "</bounce>Already Prayed");
        prayButton.interactable =!alreadyPrayed.Contains(party.ActiveUnit);
        characterFacesContainers.transform.DeleteAllChildren();
        godsContainers.transform.DeleteAllChildren(); //TODO Do this only at show not each time
        uiGodBlessings.Clear();
        for (int i=0; i <gods.Count; i++)
        {
            var go = Instantiate(godTabPrefab, godsContainers.transform);
            go.GetComponent<UIGodBlessing>().Show(party.ActiveUnit, gods[i], i, i==selectedGod);
            go.GetComponent<UIGodBlessing>().onClicked += GodClicked;
            uiGodBlessings.Add(go.GetComponent<UIGodBlessing>());
        }
        foreach (var member in party.members)
        {
            var go = Instantiate(characterFacePrefab, characterFacesContainers.transform);
            go.GetComponent<UICharacterFace>().Show(member);
            if(member.Equals(party.ActiveUnit))
                go.GetComponent<UICharacterFace>().Select();
            go.GetComponent<UICharacterFace>().onClicked += CharacterClicked;
        }
    }

    int GoldToExp(int gold, int faith)
    {
        
        return (int)((gold*goldExpConvert)*(1+((faith*faithExpMult)/100f)));
    }

    Unit GetUnitFromGod(God god)
    {
        foreach (var member in party.members)
        {
            if (member.Blessing != null && member.Blessing.God == god)
            {
                return member;
            }
        }

        return null;
    }

    void CharacterClicked(Unit unit)
    {
        Player.Instance.Party.SetActiveUnit( unit);
    }
    void GodClicked(int index)
    {
        prevGod = selectedGod;
        uiGodBlessings[selectedGod].Deselect();
        selectedGod = index;
        uiGodBlessings[selectedGod].Select();
        UpdateUI();
        bondExpBar.SetBar01(party.ActiveUnit.Bonds.GetBondExperience(gods[selectedGod])/100f);
    }
    public void NextClicked()
    {
        Player.Instance.Party.ActiveUnitIndex++;
        
    }

    public void PrevClicked()
    {
      
        Player.Instance.Party.ActiveUnitIndex--;
        
    }
    public void NextGodClicked()
    {
        prevGod = selectedGod;
        selectedGod++;
        if (selectedGod >= gods.Count)
            selectedGod = gods.Count - 1;
        UpdateUI();
        bondExpBar.SetBar01(party.ActiveUnit.Bonds.GetBondExperience(gods[selectedGod])/100f);

    }

    public void PrevGodClicked()
    {
        prevGod = selectedGod;
        selectedGod--;
        if (selectedGod < 0)
            selectedGod = 0;
        UpdateUI();
        bondExpBar.SetBar01(party.ActiveUnit.Bonds.GetBondExperience(gods[selectedGod])/100f);
    }

    public void Hide()
    {
        shrineController.Hide();
        canvas.enabled = false;
        party.onActiveUnitChanged -= ActiveUnitChanged;
      
    }

    private void OnDestroy()
    {
        party.onActiveUnitChanged -= ActiveUnitChanged;
    }

    void ActiveUnitChanged()
    {
        UpdateUI();
        bondExpBar.SetBar01(party.ActiveUnit.Bonds.GetBondExperience(gods[selectedGod])/100f);
        
    }
    public void Show(ChurchEncounterNode node, Party party)
    {
        Debug.Log("Showing church ui screen");
        this.node = node;
        canvas.enabled = true;
        this.party = party;
        this.church = node.church;
        shrineController.Show(gods);
        party.onActiveUnitChanged -= ActiveUnitChanged;
        party.onActiveUnitChanged += ActiveUnitChanged;
        alreadyPrayed = new List<Unit>();
        bondExpBar.SetBar01(party.ActiveUnit.Bonds.GetBondExperience(gods[selectedGod])/100f);
        UpdateUI();
    }
    
    public void ContinueClicked()
    {
        canvas.enabled = false;
        FindObjectOfType<UICharacterViewController>().Hide();
        node.Continue();
    }

   

    public void PrayClicked()
    {
        alreadyPrayed.Add(party.ActiveUnit);
        party.ActiveUnit.Bonds.Increase(gods[selectedGod], party.ActiveUnit.Stats.CombinedAttributes().FAITH);
        UpdateUI();
    }
    public void DonateClicked()
    {
        party.AddGold(-100);
        party.ActiveUnit.Bonds.Increase(gods[selectedGod],donateExtraExp+ party.ActiveUnit.Stats.CombinedAttributes().FAITH);
        UpdateUI();
        
    }

    public void ReceiveBlessingClicked()
    {
        var godUnit = GetUnitFromGod(gods[selectedGod]);
        if (party.ActiveUnit.Blessing == null && godUnit == null)
        {
            if(party.ActiveUnit.CanReceiveBlessing(gods[selectedGod]))
                party.ActiveUnit.ReceiveBlessing(gods[selectedGod].GetBlessing());
        }
        else if (godUnit!=null&&godUnit.Equals(party.ActiveUnit))
        {
            okCancelDialogController.Show("Do you want to break the champion bond?",()=> party.ActiveUnit.RemoveBlessing());
           
        }else if(party.ActiveUnit.Blessing == null)
        {
            if (party.ActiveUnit.CanReceiveBlessing(gods[selectedGod]))
            {
                okCancelDialogController.Show("Do you want to break the existing champion bond?", () =>
                {

                    godUnit.RemoveBlessing();
                    party.ActiveUnit.ReceiveBlessing(gods[selectedGod].GetBlessing());
                });
            }

        }
        
        UpdateUI();
    }
    

    public void RemoveCurse()
    {
        party.ActiveUnit.RemoveAllCurses();
        party.AddGold(-100);
        
        UpdateUI();
        Debug.Log("Remove Curse");
    }
}
