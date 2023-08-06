using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Mechanics;
using Game.Mechanics.Battle;
using UnityEngine;

namespace LostGrace
{
    public class UIBattleStatsBottom : MonoBehaviour
    {
        [SerializeField] UIStatText Atk;
        [SerializeField] UIStatText AtkSpeed;
        [SerializeField] UIStatText PhysArmor;
        [SerializeField] UIStatText MagicArmor;
        [SerializeField] UIStatText Hitrate;
        [SerializeField] UIStatText DodgeRate;
        [SerializeField] UIStatText Crit;
        [SerializeField] UIStatText CritAvoid;
        private Unit unit;
        public void Show(Unit unit)
        {
            this.unit = unit;
            bool physical = unit.equippedWeapon.DamageType == DamageType.Physical;
        int sumBonuses = unit.Stats.GetCombatStatBonuses(unit,BonusStats.CombatStatType.Attack,physical);
        Atk.SetValue(unit.BattleComponent.BattleStats.GetDamage(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,BonusStats.CombatStatType.AttackSpeed,physical);
        AtkSpeed.SetValue(unit.BattleComponent.BattleStats.GetAttackSpeed(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,BonusStats.CombatStatType.PhysicalResistance,physical);
        PhysArmor.SetValue(unit.BattleComponent.BattleStats.GetPhysicalResistance(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,BonusStats.CombatStatType.MagicResistance,physical);
        MagicArmor.SetValue(unit.BattleComponent.BattleStats.GetFaithResistance(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,BonusStats.CombatStatType.Hit,physical);
        Hitrate.SetValue(unit.BattleComponent.BattleStats.GetHitrate(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,BonusStats.CombatStatType.Avoid,physical);
        DodgeRate.SetValue(unit.BattleComponent.BattleStats.GetAvoid(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,BonusStats.CombatStatType.Crit,physical);
        Crit.SetValue(unit.BattleComponent.BattleStats.GetCrit(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,BonusStats.CombatStatType.Critavoid,physical);
        CritAvoid.SetValue(unit.BattleComponent.BattleStats.GetCritAvoid(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);

        }

        public void StatClicked(int index)
        {
            Vector3 position = new Vector3();
            switch ((BonusStats.CombatStatType)index)
            {
                case BonusStats.CombatStatType.Attack: position = Atk.transform.position;
                    break;
                case BonusStats.CombatStatType.AttackSpeed: position = Atk.transform.position; break;
                case BonusStats.CombatStatType.Hit: position = Hitrate.transform.position; break;
                case BonusStats.CombatStatType.Avoid: position = DodgeRate.transform.position; break;
                case BonusStats.CombatStatType.Crit: position = Crit.transform.position; break;
                case BonusStats.CombatStatType.Critavoid: position = CritAvoid.transform.position; break;
                case BonusStats.CombatStatType.MagicResistance: position = MagicArmor.transform.position; break;
                case BonusStats.CombatStatType.PhysicalResistance: position = PhysArmor.transform.position; break;
            }
            ToolTipSystem.ShowCombatStatValue(unit, (BonusStats.CombatStatType)index, position);
        }
    }
}
