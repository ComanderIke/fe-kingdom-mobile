using System.Collections;
using System.Collections.Generic;
using Game.AI;
using TMPro;
using UnityEngine;

namespace LostGrace
{
    public class BattleResultRenderer : MonoBehaviour
    {
        [SerializeField] private Transform rewardLineParent;
        [SerializeField] private GameObject rewardLinePrefab;
        [SerializeField] private TextMeshProUGUI totalBexp;
        [SerializeField] private TextMeshProUGUI totalGold;
        [SerializeField] private Animator rewardChestAnimator;
        [SerializeField] private Animator mvp;
        [SerializeField] private ResultMVPPanelUI resultmvpPanel;
        private static readonly int Open = Animator.StringToHash("Open");

        public void Show(BattleResult result)
        {
            if (result.GetTurnCount() != 0)
            {
                var lineGO=Instantiate(rewardLinePrefab, rewardLineParent);
                var rewardLine = lineGO.GetComponent<RewardLineUI>();
                rewardLine.SetValues("Turn Count", result.GetGoldFromTurnCount(), result.GetExpFromTurnCount(), result.GetTurnCount(), null);
            }
            if (result.GetEnemyCount() != 0)
            {
                var lineGO=Instantiate(rewardLinePrefab, rewardLineParent);
                var rewardLine = lineGO.GetComponent<RewardLineUI>();
                rewardLine.SetValues("Enemies", result.GetGoldFromEnemies(), result.GetExpFromEnemies(), result.GetEnemyCount(), result.GetFirstEnemy().visuals.Prefabs.UIAnimatorController);
            }
            if (result.GetEliteEnemyCount() != 0)
            {
                var lineGO=Instantiate(rewardLinePrefab, rewardLineParent);
                var rewardLine = lineGO.GetComponent<RewardLineUI>();
                rewardLine.SetValues("Elite Enemies", result.GetGoldFromEliteEnemies(), result.GetExpFromEliteEnemies(), result.GetEliteEnemyCount(),result.GetFirstEliteEnemy().visuals.Prefabs.UIAnimatorController);
            }

            totalBexp.text = ""+result.GetTotalBexp();
            totalGold.text = ""+result.GetTotalGold();
            mvp.runtimeAnimatorController = result.GetMVP().visuals.Prefabs.UIAnimatorController;
            rewardChestAnimator.SetBool(Open, false);
            resultmvpPanel.Show(result.GetMVP(), result.GetGachaReward());

        }
        public void OnChestClicked()
        {
            rewardChestAnimator.SetBool(Open, true);
        }
    }
}
