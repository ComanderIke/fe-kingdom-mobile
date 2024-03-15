using Game.GameActors.Units;
using Game.GUI.ToolTips;
using Game.States.Mechanics.Battle;
using Game.Systems;
using UnityEngine;

namespace Game.GUI.Other
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
        int sumBonuses = unit.Stats.GetCombatStatBonuses(unit,CombatStats.CombatStatType.Attack,physical);
        Atk.SetValue(unit.BattleComponent.BattleStats.GetDamage(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,CombatStats.CombatStatType.AttackSpeed,physical);
        AtkSpeed.SetValue(unit.BattleComponent.BattleStats.GetAttackSpeed(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,CombatStats.CombatStatType.Protection,physical);
        PhysArmor.SetValue(unit.BattleComponent.BattleStats.GetPhysicalResistance(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,CombatStats.CombatStatType.Resistance,physical);
        MagicArmor.SetValue(unit.BattleComponent.BattleStats.GetFaithResistance(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,CombatStats.CombatStatType.Hit,physical);
        Hitrate.SetValue(unit.BattleComponent.BattleStats.GetHitrate(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,CombatStats.CombatStatType.Avoid,physical);
        DodgeRate.SetValue(unit.BattleComponent.BattleStats.GetAvoid(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,CombatStats.CombatStatType.Crit,physical);
        Crit.SetValue(unit.BattleComponent.BattleStats.GetCrit(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,CombatStats.CombatStatType.CurseResistance,physical);
        CritAvoid.SetValue(unit.BattleComponent.BattleStats.GetCurseResistance(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);

        }

        public void StatClicked(int index)
        {
            Vector3 position = new Vector3();
            switch ((CombatStats.CombatStatType)index)
            {
                case CombatStats.CombatStatType.Attack: position = Atk.transform.position;
                    break;
                case CombatStats.CombatStatType.AttackSpeed: position = Atk.transform.position; break;
                case CombatStats.CombatStatType.Hit: position = Hitrate.transform.position; break;
                case CombatStats.CombatStatType.Avoid: position = DodgeRate.transform.position; break;
                case CombatStats.CombatStatType.Crit: position = Crit.transform.position; break;
                case CombatStats.CombatStatType.CurseResistance: position = CritAvoid.transform.position; break;
                case CombatStats.CombatStatType.Resistance: position = MagicArmor.transform.position; break;
                case CombatStats.CombatStatType.Protection: position = PhysArmor.transform.position; break;
            }
            ToolTipSystem.ShowCombatStatValue(unit, (CombatStats.CombatStatType)index, position);
        }
    }
}
