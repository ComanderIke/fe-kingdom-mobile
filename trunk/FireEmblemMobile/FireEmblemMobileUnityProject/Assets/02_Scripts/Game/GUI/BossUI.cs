using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units;
using Game.GUI;
using Game.WorldMapStuff.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
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
            this.unit = unit;
            canvas.enabled=true;
            hpBar.SetEnemyColors();
            hpBar.UpdateValuesWithoutDamagePreview(unit.MaxHp, unit.Hp, unit.Hp);
            UpdateValues();
            unit.HpValueChanged -= UpdateValues;
            unit.HpValueChanged += UpdateValues;
           
        }

        void UpdateValues()
        {
            nameText.text = unit.name;
            faceImage.sprite = unit.visuals.CharacterSpriteSet.FaceSprite;
            Debug.Log("SHOW BOSS UI " + unit.MaxHp+" "+unit.Hp);
            hpBar.UpdateValuesAnimated(unit.MaxHp, unit.Hp);
            revivalStonesContainer.DeleteAllChildren();
            for (int i = 0; i < unit.RevivalStones; i++)
            {
                Instantiate(revivalStonePrefab, revivalStonesContainer);
            }
            if (unit.AIComponent.AIBehaviour is MinotaurAIBehaviour minotaurAIBehaviour && minotaurAIBehaviour.GetMaxRageMeter() != 0)
            {
                rageUI.Show(minotaurAIBehaviour);
                
            }
        }

        private void OnDisable()
        {
            if(unit!=null)
                unit.HpValueChanged -= UpdateValues; 
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
