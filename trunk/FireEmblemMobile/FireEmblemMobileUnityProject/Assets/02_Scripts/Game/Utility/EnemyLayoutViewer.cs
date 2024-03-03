using System.Collections.Generic;
using Game.EncounterAreas.Encounters.Battle;
using Game.LevelDesign;
using UnityEngine;

namespace Game.Utility
{
    public class EnemyLayoutViewer : MonoBehaviour
    {
        public BattleMap map;

        // public Transform container;

        // public GameObject unitPrefab;

        private List<UnitSpawner> spawnedUnitLayouts;

        private GameObject battleMapInstantiation;
        // Start is called before the first frame update
        void Start()
        {
            Hide();
        }
    

        public void Show()
        {
            Hide();
            // spawnedUnitLayouts
            //     = new List<UnitSpawner>();
            // foreach (var unit in enemyLayout.unitLayout)
            // {
            //     var go = Instantiate(unitPrefab, container);
            //     go.transform.localPosition = new Vector3(unit.spawnPosition.x,unit.spawnPosition.y,0);
            //     go.GetComponent<UnitSpawner>().spriteRenderer.sprite = unit.unit.visuals.CharacterSpriteSet.MapSprite;
            //     spawnedUnitLayouts.Add(go.GetComponent<UnitSpawner>());
            // }

            battleMapInstantiation = Instantiate(map.mapPrefab, null);
        }

        public void Hide()
        {
            // if(spawnedUnitLayouts!=null)
            //     spawnedUnitLayouts.Clear();
            // container.DeleteAllChildrenImmediate();
            DestroyImmediate(battleMapInstantiation);
        }
        public void ApplyChanges()
        {
            // if (spawnedUnitLayouts != null)
            // {
            //     int cnt = 0;
            //     foreach (var unit in enemyLayout.unitLayout)
            //     {
            //         unit.spawnPosition = new Vector2(spawnedUnitLayouts[cnt].X, spawnedUnitLayouts[cnt].Y);
            //         cnt++;
            //     }
            // }
        }
    }
}
