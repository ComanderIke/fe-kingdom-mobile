using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterViewController : MonoBehaviour
{
    public TextMeshProUGUI charName;
    public TextMeshProUGUI Lv;
    public TextMeshProUGUI Exp;
    public TextMeshProUGUI CharClass;
    public Image sprite;
    public Image weaponIcon;
    public TextMeshProUGUI weaponAtk;
    public TextMeshProUGUI Atk;
    public TextMeshProUGUI AS;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI MaxHP;
    public TextMeshProUGUI SP;
    public TextMeshProUGUI STR;
    public TextMeshProUGUI MAG;
    public TextMeshProUGUI AGI;
    public TextMeshProUGUI DEF;
    public TextMeshProUGUI RES;
    public IStatBar HPBar;
    public ISPBarRenderer SPBars;
    public void Show(Unit partyMember)
    {
        
    }
}
