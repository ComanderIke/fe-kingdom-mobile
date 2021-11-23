using Game.GameResources;
using Game.Graphics;
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
        public GridData gridData;
        private int width;
        private int height;
        private Tile[,] tiles;
        public Transform gridTransform;
        private bool initialized;

        void Awake()
        {
            gridData = GameAssets.Instance.grid.gridData;
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
            width = gridData.width;
            height = gridData.height;
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