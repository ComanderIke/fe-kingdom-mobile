using Assets.Mechanics.Battle;
using Assets.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI
{
    public class AttackPreviewUI : MonoBehaviour
    {
        [Header("Left")]
        [SerializeField] private TextMeshProUGUI atkValue = default;
        [SerializeField] private TextMeshProUGUI spdValue = default;
        [SerializeField] private TextMeshProUGUI defLabel = default;
        [SerializeField] private TextMeshProUGUI defValue = default;
        [SerializeField] private TextMeshProUGUI sklValue = default;
        [SerializeField] private Image faceSpriteLeft = default;
        [SerializeField] private TextMeshProUGUI dmgValue = default;
        [SerializeField] private GameObject attackCount = default;
        [SerializeField] private AttackPreviewStatBar hpBar = default;
        [SerializeField] private AttackPreviewStatBar spBar = default;
        [Header("Right")]
        [SerializeField] private TextMeshProUGUI atkValueRight = default;
        [SerializeField] private TextMeshProUGUI spdValueRight = default;
        [SerializeField] private TextMeshProUGUI defLabelRight = default;
        [SerializeField] private TextMeshProUGUI defValueRight = default;
        [SerializeField] private TextMeshProUGUI sklValueRight = default;
        [SerializeField] private Image faceSpriteRight = default;
        [SerializeField] private TextMeshProUGUI dmgValueRight = default;
        [SerializeField] private GameObject attackCountRight = default;
        [SerializeField] private AttackPreviewStatBar hpBarRight = default;
        [SerializeField] private AttackPreviewStatBar spBarRight = default;

        public void UpdateValues ( BattlePreview battlePreview, Sprite attackerSprite, Sprite defenderSprite)
        {
            atkValue.text= "" + battlePreview.Attacker.Attack;
            spdValue.text = "" + battlePreview.Attacker.Speed;
            defLabel.text = battlePreview.Attacker.IsPhysical ? "Def" : "Res";
            defValue.text = "" + battlePreview.Attacker.Defense;
            sklValue.text = "" + battlePreview.Attacker.Skill;
            faceSpriteLeft.sprite = attackerSprite;
            dmgValue.text = "" + battlePreview.Attacker.Damage;
            attackCount.SetActive(battlePreview.Attacker.AttackCount>1);
            hpBar.UpdateValues(battlePreview.Attacker.MaxHp, battlePreview.Attacker.CurrentHp, battlePreview.Attacker.AfterBattleHp, battlePreview.Attacker.IncomingDamage);
            spBar.UpdateValues(battlePreview.Attacker.MaxSp, battlePreview.Attacker.CurrentSp, battlePreview.Attacker.AfterBattleSp, battlePreview.Attacker.IncomingSpDamage);

            atkValueRight.text = "" + battlePreview.Defender.Attack;
            spdValueRight.text = "" + battlePreview.Defender.Speed;
            defLabelRight.text = battlePreview.Defender.IsPhysical ? "Def" : "Res";
            defValueRight.text = "" + battlePreview.Defender.Defense;
            sklValueRight.text = "" + battlePreview.Defender.Skill;
            faceSpriteRight.sprite = defenderSprite;
            dmgValueRight.text = "" + battlePreview.Defender.Damage;
            attackCountRight.SetActive(battlePreview.Defender.AttackCount > 1);
            hpBarRight.UpdateValues(battlePreview.Defender.MaxHp, battlePreview.Defender.CurrentHp, battlePreview.Defender.AfterBattleHp, battlePreview.Defender.IncomingDamage);
            spBarRight.UpdateValues(battlePreview.Defender.MaxSp, battlePreview.Defender.CurrentSp, battlePreview.Defender.AfterBattleSp, battlePreview.Defender.IncomingSpDamage);

        }
    }
}