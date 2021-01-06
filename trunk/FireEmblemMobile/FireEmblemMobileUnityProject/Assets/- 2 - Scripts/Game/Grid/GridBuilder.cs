using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Grid
{
    public class GridBuilder:MonoBehaviour
    {
        private const string CELL_NAME = "Grid Cell";
        private const string CELL_TAG = "Grid";
        private const int CELL_LAYER = 10;
        private const string BLOCK_FIELD_LAYER = "BlockField";

        public GridData gridData;
        private int width;
        private int height;
        private Tile[,] tiles;
        public Sprite gridSprite;
        private TileTypeAnalyzer tileTypeAnalyzerAnalyzer;

        [SerializeField]
        private TileType baseTile;
        public Material gridMaterial;
        public Transform gridTransform;
        
        public void Build(int width, int height)
        {
            foreach (var transform in gridTransform.GetComponentsInChildren<Transform>())
            {
                if (transform.gameObject != gridTransform.gameObject)
                    Object.DestroyImmediate(transform.gameObject);
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var cell = CreateGridTileGameObject(i, j);
                    var meshRenderer = cell.GetComponent<SpriteRenderer>();
                    meshRenderer.material = gridMaterial;
                }
            }
        }
        public Tile[,] GetTiles()
        {
            width = gridData.width;
            height = gridData.height;
            tiles = new Tile[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var cell = gridTransform.GetChild(i * width + j).gameObject;
                    var tileData = cell.GetComponent<TileData>();
                    if (tileData == null)
                    {
                        cell.AddComponent<TileData>().tileType = default;
                    }

                    tiles[i, j] = new Tile(i, j, tileData.tileType, cell);
                }
            }


            return tiles;
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
            spriteRenderer.sprite = gridSprite;
            spriteRenderer.sortingOrder = 0;
            go.AddComponent<BoxCollider2D>().size = new Vector3(1, 1);
            go.AddComponent<TileData>().tileType=baseTile;
            
            return go;
        }
        

  

    }
}