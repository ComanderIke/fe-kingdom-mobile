using Assets.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI
{
    public class AttackPreview : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hpText = default;
        [SerializeField] private TextMeshProUGUI hpEnemyText = default;
        [SerializeField] private TextMeshProUGUI dmgText = default;
        [SerializeField] private TextMeshProUGUI attackCountText = default;
        [SerializeField] private Image hpBar = default;
        [SerializeField] private Image hpBarEnemy = default;

        public void UpdateValues(int maxHp, int hp, int maxHpEnemy, int hpEnemy, int dmg, int attackCount)
        {
            dmgText.text = "" + dmg;
            attackCountText.text = " x " + attackCount;
            hpText.text = "" + hp;
            hpEnemyText.text = "" + hpEnemy;
            hpBar.fillAmount = MathUtility.MapValues(hp, 0, maxHp, 0, 1);
            hpBarEnemy.fillAmount = MathUtility.MapValues(hpEnemy, 0, maxHpEnemy, 0, 1);
        }
    }
}