using System;
using System.Collections.Generic;
using System.Linq;
using Game.Grid;
using UnityEngine;

namespace Game.AI
{
    public class TileVisualizer:MonoBehaviour
    {
        private List<GameObject> instantiatedTileEffects;
        [SerializeField]private GameObject tileEffectprefabBlue;
        [SerializeField]private GameObject tileEffectprefabRed;

        private void Start()
        {
            instantiatedTileEffects = new List<GameObject>();
        }

        public void Show(GameObject prefab, Vector2Int moveOption)
        {
           var go= Instantiate(prefab, transform);
           go.transform.position = new Vector3(moveOption.x+.5f, moveOption.y+.5f, 0);
           instantiatedTileEffects.Add(go);
        }

        public void Hide()
        {
            for (int i = instantiatedTileEffects.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(instantiatedTileEffects[i]);
            }
            instantiatedTileEffects.Clear();
        }

        public void ShowRed(Vector2Int pos)
        {
            Show(tileEffectprefabRed, pos);
        }
        public void ShowBlue(Vector2Int pos)
        {
            Show(tileEffectprefabBlue, pos);
        }
    }
}