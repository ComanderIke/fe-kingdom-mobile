using Game.GameActors.Units;
using Game.GUI.Other;
using Game.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Game.GUI
{
    public class BossUI : MonoBehaviour
    {
        [SerializeField] private AttackPreviewStatBar hpBar;

        [SerializeField] private Image faceImage;

        [SerializeField] private TextMeshProUGUI nameText;

        [SerializeField] private Transform revivalStonesContainer;

        [SerializeField] private GameObject revivalStonePrefab;

        [SerializeField] private MinotaurRageMeterUI rageUI;
        [SerializeField] private Canvas canvas;
        private Unit unit;
        // Start is called before the first frame update
        public void Show(Unit unit)
        {
            if (canvas.enabled)
                return;
            this.unit = unit;
            canvas.enabled=true;
            nameText.text = unit.name;
            faceImage.sprite = unit.visuals.CharacterSpriteSet.FaceSprite;
            hpBar.SetEnemyColors();
            hpBar.UpdateValuesWithoutDamagePreview(unit.MaxHp, unit.Hp, unit.Hp);
            UpdateHPValues();
            UpdateRevivalStones();
            unit.HpValueChanged -= UpdateHPValues;
            unit.HpValueChanged += UpdateHPValues;
            unit.RevivalStonesChanged -= UpdateRevivalStones;
            unit.RevivalStonesChanged += UpdateRevivalStones;
            Unit.UnitDied -= CheckBossDied;
            Unit.UnitDied += CheckBossDied;
            if (unit.AIComponent.AIBehaviour is MinotaurAIBehaviour minotaurAIBehaviour && minotaurAIBehaviour.GetMaxRageMeter() != 0)
            {
                rageUI.Show(minotaurAIBehaviour);
                
            }

        }

        void Hide()
        {
            if (unit != null)
            {
                unit.RevivalStonesChanged -= UpdateRevivalStones;
                unit.HpValueChanged -= UpdateHPValues; 
            }
                
            Unit.UnitDied -= CheckBossDied;
            canvas.enabled = false;
        }
        void CheckBossDied(Unit unit)
        {
            if (unit.Equals(this.unit))
            {
                Hide();
            }
        }

        private int currentUnitHp = 0;
        void UpdateHPValues()
        {
            if (unit.Hp == currentUnitHp)
                return;
            MyDebug.LogTODO("THis Method is called too often.");
            MyDebug.LogTODO("Only Animation if hp count changed!");
            AnimationQueue.Add(() =>
            {
                hpBar.UpdateValuesAnimated(unit.MaxHp, unit.Hp);
                MonoUtility.DelayFunction(()=>
                    AnimationQueue.OnAnimationEnded?.Invoke(), 1.5f);
            });
            currentUnitHp = unit.Hp;
        }

        private int currentRevivalStones = -1;
        void UpdateRevivalStones()
        {
            if (unit.RevivalStones == currentRevivalStones)
                return;
            MyDebug.LogTODO("Only Animation if revival stone count changed!");
            AnimationQueue.Add(() =>
            {
                revivalStonesContainer.DeleteAllChildren();
                for (int i = 0; i < unit.RevivalStones; i++)
                {
                    Instantiate(revivalStonePrefab, revivalStonesContainer);
                }
                MonoUtility.DelayFunction(()=>
                    AnimationQueue.OnAnimationEnded?.Invoke(), .5f);
            });
            currentRevivalStones = unit.RevivalStones;
        }

        private void OnDisable()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
