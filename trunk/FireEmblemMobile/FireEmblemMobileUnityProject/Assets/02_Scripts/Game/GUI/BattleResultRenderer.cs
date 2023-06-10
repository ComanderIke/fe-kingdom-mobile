using System;
using System.Collections;
using System.Collections.Generic;
using Game.AI;
using Game.GameActors.Players;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;

namespace LostGrace
{
    [ExecuteInEditMode]
    public class BattleResultRenderer : MonoBehaviour
    {
        [SerializeField] private Transform rewardLineParent;
        [SerializeField] private GameObject rewardLinePrefab;
        [SerializeField] private TextMeshProUGUI totalBexp;
        [SerializeField] private TextMeshProUGUI totalGold;
        [SerializeField] private float normalHeight;
        [SerializeField] private float extendedHeight;
        [SerializeField] private bool extended = false;
        [SerializeField] private RectTransform panel;
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasGroup canvasGroup;
        public event Action OnFinished;

        private void OnEnable()
        {
            if (extended)
            {
                panel.offsetMin = new Vector2(panel.offsetMin.x, extendedHeight);
            }
            else
            {
                panel.offsetMin = new Vector2(panel.offsetMin.x, normalHeight);
            }
        }

        public void Show(BattleResult result)
        {

            canvas.enabled = true;
            TweenUtility.FadeIn(canvasGroup);
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

        }

      
        
        public void Hide()
        {
            Debug.Log("HIDE RESULT PANNEL");
            TweenUtility.FadeOut(canvasGroup).setOnComplete(() =>
            {
                Debug.Log("FADEOUT FINISHED");
                canvas.enabled = false;
                OnFinished?.Invoke();
            });
        }
    
    }
}
