﻿using System;
using Game.Grid.Tiles;
using UnityEngine;

namespace Game.Graphics.Grid
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Tile/TileEffectVisuals", fileName =  "TileEffectVisual")]
    public class TileEffectVisual : ScriptableObject
    {
       
        private GameObject tileEffectGO;

        [SerializeField]
        private GameObject tileEffectPrefab;
        

        private void OnEnable()
        {
            // BattleState.OnEnter += HideAttackableField;
            // MovementState.OnEnter += HideAttackableField;
            // UnitSelectionSystem.OnDeselectCharacter += HideAttackableField;
            
        }

        public void Show(Tile tile)
        {
            if (tileEffectGO !=null)
            {
                
                tileEffectGO.SetActive(true);
            }
            else
            {
                Create(tile.GetTransform());
            }
            tileEffectGO.transform.localPosition =  new Vector3(0.0f,0.0f,0);
        }

        private void Create(Transform parentTransform)
        {
            tileEffectGO = Instantiate(tileEffectPrefab, parentTransform);

        }

        public void Hide()
        {
            if(tileEffectGO!=null)
                tileEffectGO.SetActive(false);
        }

      
    }
}