﻿using System;
using Game.GameResources;
using Game.Graphics;
using Game.Manager;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Grid
{
    public class GridBuilder:MonoBehaviour
    {
        private const string CELL_NAME = "Grid Cell";
        private const string CELL_TAG = "Grid";
        private const int CELL_LAYER = 10;

[HideInInspector]
        public int width;
        [HideInInspector]
        public int height;
        private Tile[,] tiles;
        public Transform gridTransform;
        private bool initialized;

        public void OnDrawGizmos()
        {
            // Gizmos.color = new Color(1, 0, 0, 0.5f);
            // Gizmos.DrawWireCube(transform.position+new Vector3(width/2f, height/2f), new Vector3(width, height, 1));
            Gizmos.color = new Color(.6f, .4f, .3f, 0.5f);
            Gizmos.DrawWireCube(transform.position+new Vector3((width+6)/2f-3, (height+2)/2f-1), new Vector3(width+6, height+2, 1));
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                   
                    Gizmos.DrawWireCube(transform.position+new Vector3(x+0.5f, y+0.5f), new Vector3(1, 1, 1));
                }
            }
          
        }

        public void Build(int width, int height)
        {
            foreach (var transform in gridTransform.GetComponentsInChildren<Transform>())
            {
                if (transform.gameObject != gridTransform.gameObject)
                    DestroyImmediate(transform.gameObject);
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var cell = CreateGridTileGameObject(i, j);
                    var meshRenderer = cell.GetComponent<SpriteRenderer>();
                    meshRenderer.material = GameAssets.Instance.grid.gridMaterial;
                }
            }
        }
        public Tile[,] GetTiles()
        {
            if(!initialized)
                InitTiles();
            return tiles;
        }

        private void InitTiles()
        {
            width = GridGameManager.Instance.BattleMap.width;
            height = GridGameManager.Instance.BattleMap.height;
            tiles = new Tile[width, height];
            var tileeffectVisualRenderer = FindObjectOfType<TileEffectRenderer>();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var cell = gridTransform.GetChild(i * height + j).gameObject;
                    tiles[i, j] = new Tile(i, j, cell.transform, new SpriteTileRenderer(cell.GetComponent<SpriteRenderer>(), GameAssets.Instance.tiles.tileSpriteSets),tileeffectVisualRenderer);
                }
            }

            initialized = true;
        }

        private GameObject CreateGridTileGameObject(int x, int y)
        {
            var go = new GameObject
            {
                layer = CELL_LAYER, tag = CELL_TAG, name = CELL_NAME + " " + x + " " + y
            };
            go.transform.parent = gridTransform.transform;
            go.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, 0);
            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = GameAssets.Instance.grid.standardSprite;
            spriteRenderer.sortingOrder = 0;
            go.AddComponent<BoxCollider2D>().size = new Vector3(1, 1);

            return go;
        }
        

  

    }
}