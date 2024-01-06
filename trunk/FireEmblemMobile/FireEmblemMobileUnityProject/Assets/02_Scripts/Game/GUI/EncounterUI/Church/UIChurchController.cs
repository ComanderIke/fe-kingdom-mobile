using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChurchController : MonoBehaviour
{
    public enum ChurchUIState
    {
        Blessing, Curse
    }
    public Canvas canvas;
    public ChurchEncounterNode node;
    [HideInInspector]
    public Party party;
    [SerializeField] private UICharacterFace characterFace;
    [SerializeField] private TextMeshProUGUI godStatueNameText;
    [SerializeField] private TextMeshProUGUI Bonusestext;
    [SerializeField] private Image godStatueImage;
    [SerializeField] private UIUnitIdleAnimation unitIdleAnimation;
    private Church church;
    [SerializeField] UIRemoveCurseArea removeCurseUI;
    [SerializeField] GameObject prayUI;
    [SerializeField] private Button blessingButton;
    [SerializeField] private Button curseButton;
    [SerializeField] private TextMeshProUGUI faithStat;
    private ChurchUIState state = ChurchUIState.Blessing;

    [SerializeField] private Transform topRowParentLayout;
    [SerializeField] private Transform bottomRowLayout;
    [SerializeField]
    private GameObject GodBlessingUIPrefab;

    [SerializeField] private List<God> gods;
    [SerializeField] private TextMeshProUGUI prayGoldAmount;
    private List<UIGodBlessing> uiGodBlessings;
    [SerializeField] private Slider slider;
    private int selectedGod = 0;
    [SerializeField] private Color tooExpensiveTextColor;
    [SerializeField] private Button prayButton;
    [SerializeField] private Button receiveBlessingButton;
    [SerializeField] private float goldExpConvert = .5f;
    [SerializeField] private float faithExpMult = 2f;
    public void UpdateUI()
    {

        faithStat.text = ""+party.ActiveUnit.Stats.CombinedAttributes().FAITH;
        prayGoldAmount.text = ""+slider.value;
        topRowParentLayout.DeleteAllChildren();
        bottomRowLayout.DeleteAllChildren();
        uiGodBlessings = new List<UIGodBlessing>();
        int cnt = 0;
        bool affordable = party.CanAfford((int)slider.value);
        prayGoldAmount.color = affordable ? Color.white : tooExpensiveTextColor;
        prayButton.interactable = affordable;
        prayButton.GetComponentInChildren<TextMeshProUGUI>().text = prayButton.interactable ? "<bounce>Pray" : "</bounce>Underfunded";
        receiveBlessingButton.gameObject.SetActive(false);
        godStatueNameText.text = "Shrine of " + church.GetGod().Name;
        godStatueImage.sprite = church.GetGod().DialogSpriteSet.FaceSprite;
        Bonusestext.text = church.GetGod().Name + "\nBond Exp +15%";
        if (party.CanReceiveBlessing(party.ActiveUnit, gods[selectedGod]))
        {
            receiveBlessingButton.gameObject.SetActive(true);
        }
        
        foreach (var god in gods)
        {
            var parent = cnt>=4?bottomRowLayout:topRowParentLayout;
            var go = Instantiate(GodBlessingUIPrefab, parent);
            var uiGodController= go.GetComponent<UIGodBlessing>();
            Unit blessedUnit = GetUnitFromGod(god);
            uiGodController.Show(party.ActiveUnit,god, cnt, GoldToExp((int)slider.value, party.ActiveUnit.Stats.CombinedAttributes().FAITH), selectedGod == cnt, blessedUnit);
            uiGodController.onClicked += GodClicked;
            if(cnt==selectedGod)
                uiGodController.Select();
            uiGodBlessings.Add(uiGodController);
            cnt++;
        }
        
        removeCurseUI.Hide();
        unitIdleAnimation.Show(party.ActiveUnit);
        characterFace.Show(party.ActiveUnit);
       
        if (state == ChurchUIState.Blessing)
        {
            blessingButton.interactable = false;
            curseButton.interactable = true;
            if(!prayUI.activeSelf)
                prayUI.gameObject.SetActive(true);
        }
        else if (state == ChurchUIState.Curse)
        {
            prayUI.gameObject.SetActive(false);
            blessingButton.interactable = true;
            curseButton.interactable = false;
            removeCurseUI.Show(party.ActiveUnit);
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

    void GodClicked(int index)
    {
        uiGodBlessings[selectedGod].Deselect();
        selectedGod = index;
        uiGodBlessings[selectedGod].Select();
        UpdateUI();
    }
    public void NextClicked()
    {
        Player.Instance.Party.ActiveUnitIndex++;
        
    }

    public void PrevClicked()
    {
        Player.Instance.Party.ActiveUnitIndex--;
        
    }


    public void Hide()
    {
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
        
    }
    public void Show(ChurchEncounterNode node, Party party)
    {
        Debug.Log("Showing church ui screen");
        this.node = node;
        canvas.enabled = true;
        this.party = party;
        this.church = node.church;
        party.onActiveUnitChanged -= ActiveUnitChanged;
        party.onActiveUnitChanged += ActiveUnitChanged;
        UpdateUI();
        //FindObjectOfType<UICharacterViewController>().Show(party.members[party.ActiveUnitIndex]);
    }

    
    public void BlessingClicked()
    {
        state = ChurchUIState.Blessing;
        UpdateUI();
    }
    public void CurseClicked()
    {
        state = ChurchUIState.Curse;
        UpdateUI();
    }
    public void ContinueClicked()
    {
        canvas.enabled = false;
        FindObjectOfType<UICharacterViewController>().Hide();
        node.Continue();
    }

    public void SliderValueChanged(float value)
    {
        UpdateUI();
    }
    public void PrayClicked()
    {
        party.AddGold(-(int)slider.value);
        party.ActiveUnit.Bonds.Increase(gods[selectedGod],GoldToExp((int)slider.value, party.ActiveUnit.Stats.CombinedAttributes().FAITH));
        //state = ChurchUIState.Blessing;
        UpdateUI();
        
    }

    public void ReceiveBlessingClicked()
    {
        party.ActiveUnit.ReceiveBlessing(gods[selectedGod].GetBlessing());
        UpdateUI();
    }
    

    public void RemoveCurse()
    {
        party.ActiveUnit.RemoveCurse(party.ActiveUnit.Curses[removeCurseUI.curseIndex]);
        party.AddGold(-removeCurseUI.CalculateFaithPriceReduction(party.ActiveUnit.Stats.CombinedAttributes().FAITH));
        
        UpdateUI();
        Debug.Log("Remove Curse");
    }
}
