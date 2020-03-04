using Assets.GameResources;
using UnityEngine;

namespace Assets.Grid
{
    public class GridBuilder
    {
        private const string CELL_NAME = "Grid Cell";
        private const string CELL_TAG = "Grid";
        private const int CELL_LAYER = 0;
        private const string BLOCK_FIELD_LAYER = "BlockField";

        public float CellSize = 1;
        private int width;
        private int height;
        public bool[,] BlockedFields;
        private Transform gridTransform;
        private readonly Material cellMaterialStandard;
        private readonly Material cellMaterialInvalid;
        private Tile[,] tiles;

        public GridBuilder()
        {
            cellMaterialInvalid = Object.FindObjectOfType<ResourceScript>().Materials.CellMaterialInValid;
            cellMaterialStandard = Object.FindObjectOfType<ResourceScript>().Materials.CellMaterialValid;
        }

        public Tile[,] Build(int width, int height, Transform gridTransform)
        {
            this.width = width;
            this.height = height;
            this.gridTransform = gridTransform;
            tiles = new Tile[width, height];
            foreach (var transform in gridTransform.GetComponentsInChildren<Transform>())
            {
                if (transform.gameObject != gridTransform.gameObject)
                    Object.DestroyImmediate(transform.gameObject);
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tiles[i, j] = new Tile(i, j, CreateChild(i, j));
                }
            }

            UpdateCells();
            return tiles;
        }

        private GameObject CreateChild(int xvalue, float yvalue)
        {
            var go = new GameObject
            {
                layer = CELL_LAYER, tag = CELL_TAG, name = CELL_NAME + " " + xvalue + " " + yvalue
            };
            go.transform.parent = gridTransform.transform;
            go.transform.localPosition = new Vector3(xvalue, yvalue, 0);
            go.transform.localRotation = Quaternion.identity;
            go.AddComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            var meshObj = CreateMesh();
            go.AddComponent<MeshFilter>().mesh = meshObj;
            go.GetComponent<MeshFilter>().sharedMesh.bounds =
                new Bounds(new Vector3(0, 0, 0), new Vector3(500, 500, 500)); //this is important!
            go.AddComponent<BoxCollider>().center = new Vector3(0.5f, 0.5f, 0);
            go.GetComponent<BoxCollider>().size = new Vector3(1, 1, 0.1f);
            go.layer = LayerMask.NameToLayer("Grid");
            return go;
        }

        private static Mesh CreateMesh()
        {
            var mesh = new Mesh
            {
                name = CELL_NAME,
                vertices = new Vector3[] {Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero},
                triangles = new int[] {0, 1, 2, 2, 1, 3},
                normals = new Vector3[] {Vector3.up, Vector3.up, Vector3.up, Vector3.up},
                uv = new Vector2[] {new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0)}
            };
            return mesh;
        }

        private Vector3 MeshVertex(int x, int y, float minus)
        {
            return new Vector3(x * CellSize, y * CellSize, 0);
        }

        private void UpdateCells()
        {
            GetBlockedFields();
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    var cell = tiles[x, z].GameObject;
                    var meshRenderer = cell.GetComponent<MeshRenderer>();
                    var meshFilter = cell.GetComponent<MeshFilter>();

                    meshRenderer.material = BlockedFields[x, z] ? cellMaterialInvalid : cellMaterialStandard;
                    if (BlockedFields[x, z])
                    {
                        tiles[x, z].IsAccessible = false;
                    }

                    UpdateMesh(meshFilter.sharedMesh, x, z);
                }
            }
        }

        private void UpdateMesh(Mesh mesh, int x, int y)
        {
            mesh.vertices = new Vector3[]
            {
                MeshVertex(0, 0, 0),
                MeshVertex(0, 1, 0),
                MeshVertex(1, 0, 0),
                MeshVertex(1, 1, 0),
            };
        }

        private void GetBlockedFields()
        {
            BlockedFields = new bool[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var origin = new Vector3(x * CellSize + (CellSize / 2), y * CellSize + (CellSize / 2), -5);
                    if (Physics.Raycast(gridTransform.TransformPoint(origin), Vector3.forward, out _,
                        Mathf.Infinity, LayerMask.GetMask(BLOCK_FIELD_LAYER)))
                    {
                        BlockedFields[x, y] = true;
                    }
                    else
                    {
                        BlockedFields[x, y] = false;
                    }
                }
            }
        }
    }
}