using System;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.Mechanics;
using Game.Mechanics.Battle;
using UnityEngine;

[CreateAssetMenu(menuName = "BattlePreviewEditorSO")]
public class BattlePreviewEditorSO : ScriptableObject
{
    public int attackerLevel=1;
    public int defenderLevel = 1;
    public UnitBP attacker;
    public UnitBP defender;
    public int attackerDamage;
    public int attackerHit;
    public int attackerAvo;
    public int attackerCrit;
    public int attackerHp;
    public int attackerDef;
    public int attackerRes;
    public int attackerAttackSpeed;
    public int defenderDamage;
    public int defenderHit;
    public int defenderCrit;
    public int defenderHp;
    public int defenderAvo;
    public int defenderDef;
    public int defenderRes;
    public int defenderAttackSpeed;
    private WeaponBP attackerWeapon;
    private WeaponBP defenderWeapon;

    public void OnValidate()
    {
        attackerWeapon = attacker.equippedWeaponBp;
        int scaledSTR =(int)( attacker.stats.BaseAttributes.STR + (attacker.stats.BaseGrowths.STR * attackerLevel)/100f);
        int scaledINT = (int)( attacker.stats.BaseAttributes.INT + (attacker.stats.BaseGrowths.INT * attackerLevel)/100f);
        int scaledDEX =(int)( attacker.stats.BaseAttributes.DEX + (attacker.stats.BaseGrowths.DEX * attackerLevel)/100f);
        int scaledAGI = (int)( attacker.stats.BaseAttributes.AGI + (attacker.stats.BaseGrowths.AGI * attackerLevel)/100f);
        int scaledLCK =(int)( attacker.stats.BaseAttributes.LCK + (attacker.stats.BaseGrowths.LCK * attackerLevel)/100f);
        int scaledDEF = (int)( attacker.stats.BaseAttributes.DEF + (attacker.stats.BaseGrowths.DEF * attackerLevel)/100f);
        int scaledRES =(int)( attacker.stats.BaseAttributes.FAITH + (attacker.stats.BaseGrowths.FAITH * attackerLevel)/100f);
        int scaledHP = (int)( attacker.stats.BaseAttributes.MaxHp + (attacker.stats.BaseGrowths.MaxHp * attackerLevel)/100f);
        int scaledDefenderSTR =(int)( defender.stats.BaseAttributes.STR + (defender.stats.BaseGrowths.STR * defenderLevel)/100f);
        int scaledDefenderINT = (int)( defender.stats.BaseAttributes.INT + (defender.stats.BaseGrowths.INT * defenderLevel)/100f);
        int scaledDefenderDEX =(int)( defender.stats.BaseAttributes.DEX + (defender.stats.BaseGrowths.DEX * defenderLevel)/100f);
        int scaledDefenderAGI = (int)( defender.stats.BaseAttributes.AGI + (defender.stats.BaseGrowths.AGI * defenderLevel)/100f);
        int scaledDefenderLCK =(int)( defender.stats.BaseAttributes.LCK + (defender.stats.BaseGrowths.LCK * defenderLevel)/100f);
        int scaledDefenderDEF = (int)( defender.stats.BaseAttributes.DEF + (defender.stats.BaseGrowths.DEF * defenderLevel)/100f);
        int scaledDefenderRES =(int)( defender.stats.BaseAttributes.FAITH + (defender.stats.BaseGrowths.FAITH * defenderLevel)/100f);
        int scaledDefenderHP = (int)( defender.stats.BaseAttributes.MaxHp + (defender.stats.BaseGrowths.MaxHp * defenderLevel)/100f);
        defenderAvo = scaledDefenderAGI *BattleStats.AVO_AGI_MULT;
        defenderDef = scaledDefenderDEF;
        defenderRes = scaledDefenderRES;
        if(attackerWeapon.DamageType==DamageType.Physical)
            attackerDamage = scaledSTR+ attackerWeapon.WeaponAttributes.Dmg- defenderDef;
        else
        {
            attackerDamage = scaledINT + attackerWeapon.WeaponAttributes.Dmg-defenderRes;
        }
        attackerHp = scaledHP;
        attackerCrit = scaledLCK+ attackerWeapon.WeaponAttributes.Crit;
        attackerAvo = scaledAGI*BattleStats.AVO_AGI_MULT;
        attackerHit = scaledDEX*BattleStats.HIT_DEX_MULT + attackerWeapon.WeaponAttributes.Hit- defenderAvo;
        attackerAttackSpeed = scaledAGI;
        attackerDef =scaledDEF;
        attackerRes = scaledRES;
        
        
        defenderWeapon = defender.equippedWeaponBp;
        if (defenderWeapon.DamageType == DamageType.Physical)
        {
            defenderDamage =  scaledDefenderSTR+ defenderWeapon.WeaponAttributes.Dmg-attackerDef;
        }
        else
        {
            defenderDamage =  scaledDefenderINT + defenderWeapon.WeaponAttributes.Dmg-attackerRes;
        }
        defenderHp =  scaledDefenderHP;
        defenderCrit =  scaledDefenderLCK+ defenderWeapon.WeaponAttributes.Crit;
      
        defenderHit = scaledDefenderDEX*BattleStats.HIT_DEX_MULT + defenderWeapon.WeaponAttributes.Hit-attackerAvo;
        defenderAttackSpeed = scaledDefenderAGI;
    }
}