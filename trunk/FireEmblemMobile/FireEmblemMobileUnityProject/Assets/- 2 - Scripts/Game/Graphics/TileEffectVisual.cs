using System;
using System.Collections.Generic;
using System.Linq;
using Game.Grid;
using Game.Manager;
using Game.Mechanics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;

namespace Game.Graphics
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Tile/TileEffectVisuals", fileName =  "TileEffectVisual")]
    public class TileEffectVisual : ScriptableObject
    {
       
        private GameObject attackableField;

        [SerializeField]
        private GameObject attackIconPrefab;
        

        private void OnEnable()
        {
            // BattleState.OnEnter += HideAttackableField;
            // MovementState.OnEnter += HideAttackableField;
            // UnitSelectionSystem.OnDeselectCharacter += HideAttackableField;
            
        }

        public void ShowAttackableField(Tile tile)
        {
            if (attackableField !=null)
            {
                
                attackableField.SetActive(true);
            }
            else
            {
                CreateAttackableField(tile.GetTransform());
            }
            attackableField.transform.localPosition =  Vector3.zero;
        }

        private void CreateAttackableField(Transform parentTransform)
        {
            attackableField = Instantiate(attackIconPrefab, parentTransform);

        }

        public void HideAttackableField()
        {
            if(attackableField!=null)
                attackableField.SetActive(false);
        }

      
    }
}