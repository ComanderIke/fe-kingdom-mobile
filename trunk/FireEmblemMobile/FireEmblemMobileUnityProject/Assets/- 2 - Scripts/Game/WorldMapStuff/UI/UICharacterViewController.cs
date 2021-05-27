using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
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
    public TextMeshProUGUI WeaponName;
    public IStatBar HPBar;
    public ISPBarRenderer SPBars;
    public void Show(Unit partyMember)
    {
        charName.SetText(partyMember.name);
        Lv.SetText(""+partyMember.ExperienceManager.Level);
        Exp.SetText(""+partyMember.ExperienceManager.Exp);
        CharClass.SetText("[CLASS]");
        sprite.sprite = partyMember.visuals.CharacterSpriteSet.MapSprite;
        if (partyMember is Human human)
        {
            weaponIcon.sprite = human.EquippedWeapon.Sprite;
            weaponAtk.SetText(""+human.EquippedWeapon.Dmg);
            WeaponName.SetText(human.EquippedWeapon.name);
           
        }
        Atk.SetText(""+partyMember.BattleComponent.BattleStats.GetDamage());
        AS.SetText(""+partyMember.BattleComponent.BattleStats.GetAttackSpeed());
        HP.SetText(""+partyMember.Hp);
        MaxHP.SetText("/"+partyMember.Stats.MaxHp);
        SP.SetText(""+partyMember.Sp);
        STR.SetText(""+partyMember.Stats.Str);
        DEF.SetText(""+partyMember.Stats.Def);
        RES.SetText(""+partyMember.Stats.Res);
        AGI.SetText(""+partyMember.Stats.Spd);
        MAG.SetText(""+partyMember.Stats.Mag);
        HPBar.SetValue(partyMember.Hp, partyMember.Stats.MaxHp);
        SPBars.SetValue(partyMember.SpBars, partyMember.MaxSpBars);

    }
}
