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
       
        private List<GameObject> attackableFieldEffects;

        [SerializeField]
        private GameObject attackIconPrefab;

        private Transform parentTransform;

        private void OnEnable()
        {
            attackableFieldEffects = new List<GameObject>();
            BattleState.OnEnter += HideAttackableField;
            MovementState.OnEnter += HideAttackableField;
            UnitSelectionSystem.OnDeselectCharacter += HideAttackableField;
            parentTransform = GameObject.FindWithTag(TagManager.VfxTag).transform;
        }

        private bool AttackableFieldEffectExists(int x, int y)
        {
            return attackableFieldEffects.Any(gameObj =>
            {
                Vector3 localPosition;
                return !gameObj.activeSelf ||
                       (Math.Abs((localPosition = gameObj.transform.localPosition).x - 0.5f - x) < 0.1f &&
                        Math.Abs(localPosition.y - 0.5f - y) < 0.1f);
            });
        }

        private GameObject FindAttackableFieldEffect(int x, int y)
        {
            return attackableFieldEffects.Find(gameObj =>
            {
                Vector3 localPosition;
                return Math.Abs((localPosition = gameObj.transform.localPosition).x - 0.5f - x) < 0.1f && Math.Abs(localPosition.y - 0.5f - y) < 0.1f ||
                       !gameObj.activeSelf;
            });
        }
        public void ShowAttackableField(int x, int y)
        {
            //Debug.Log("Show Attackable Field: "+x+" " +y );
            if (AttackableFieldEffectExists(x, y))
            {
                GameObject go2 = FindAttackableFieldEffect(x, y);
                go2.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, go2.transform.localPosition.z);
                go2.SetActive(true);
            }
            else
            {
                CreateAttackableField(x, y);
            }
        }

        private void CreateAttackableField(int x, int y)
        {
            var go = Instantiate(attackIconPrefab,
                parentTransform);
            go.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, go.transform.localPosition.z);
            attackableFieldEffects.Add(go);
        }

        public void HideAttackableField()
        {
            foreach (var go in attackableFieldEffects)
            {
                go.SetActive(false);
            }
        }

      
    }
}