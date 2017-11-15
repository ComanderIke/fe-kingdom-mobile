using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Grid
{
    public class GridBuilder
    {
        const string CELL_NAME = "Grid Cell";
        const string CELL_TAG = "Grid";
        const int CELL_LAYER = 0;
        const string BLOCK_FIELD_LAYER = "BlockField";

        public float cellSize = 1;

        public bool[,] blockedFields;
        public GridManager GridManager { get; set; }

        public GridBuilder(GridManager gridManager)
        {
            GridManager = gridManager;
            for (int i = 0; i < gridManager.grid.width; i++)
            {
                for (int j = 0; j < gridManager.grid.height; j++)
                {
                    gridManager.Tiles[i, j] = new Tile(i, j, CreateChild(i, j));
                }
            }
            UpdateCells();
        }


        GameObject CreateChild(int xvalue, float yvalue)
        {
            GameObject go = new GameObject();
            go.layer = CELL_LAYER;
            go.tag = CELL_TAG;
            go.name = CELL_NAME + " " + xvalue + " " + yvalue;
            go.transform.parent = GridManager.transform;
            go.transform.localPosition = new Vector3(xvalue, yvalue, 0);
            go.transform.localRotation = Quaternion.identity;
            go.AddComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            Mesh meshobj = CreateMesh();
            go.AddComponent<MeshFilter>().mesh = meshobj;
            go.GetComponent<MeshFilter>().sharedMesh.bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(500, 500, 500));//this is important!
            go.AddComponent<BoxCollider>().center = new Vector3(0.5f, 0.5f, 0);
            go.GetComponent<BoxCollider>().size = new Vector3(1, 1, 0.1f);
            return go;
        }

        Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.name = CELL_NAME;
            mesh.vertices = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
            mesh.triangles = new int[] { 0, 1, 2, 2, 1, 3 };
            mesh.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
            mesh.uv = new Vector2[] { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0) };
            return mesh;
        }

        Vector3 MeshVertex(int x, int y, float minus)
        {
            return new Vector3(x * cellSize, y * cellSize, 0);
        }

        void UpdateCells()
        {
            GetBlockedFields();
            for (int z = 0; z < GridManager.grid.height; z++)
            {
                for (int x = 0; x < GridManager.grid.width; x++)
                {
                    GameObject cell = GridManager.Tiles[x, z].gameObject;
                    MeshRenderer meshRenderer = cell.GetComponent<MeshRenderer>();
                    MeshFilter meshFilter = cell.GetComponent<MeshFilter>();

                    meshRenderer.material = blockedFields[x, z] ? GridManager.gridRessources.cellMaterialInvalid : GridManager.gridRessources.cellMaterialValid;
                    if (blockedFields[x, z])
                    {
                        GridManager.Tiles[x, z].isAccessible = false;
                    }
                    UpdateMesh(meshFilter.mesh, x, z);
                }
            }
        }

        void UpdateMesh(Mesh mesh, int x, int y)
        {
            mesh.vertices = new Vector3[] {
                MeshVertex(0, 0, 0),
                MeshVertex(0, 1,0),
                MeshVertex( 1, 0,0 ),
                MeshVertex(1,  1,0 ),
            };
        }

        void GetBlockedFields()
        {
            RaycastHit hitInfo;
            Vector3 origin;
            blockedFields = new bool[GridManager.grid.width, GridManager.grid.height];
            for (int y = 0; y < GridManager.grid.height; y++)
            {
                for (int x = 0; x < GridManager.grid.width; x++)
                {
                    origin = new Vector3(x * cellSize + (cellSize / 2), y * cellSize + (cellSize / 2), -5);
                    if (Physics.Raycast(GridManager.transform.TransformPoint(origin), Vector3.forward, out hitInfo, Mathf.Infinity, LayerMask.GetMask(BLOCK_FIELD_LAYER)))
                    {
                        blockedFields[x, y] = true;
                    }
                    else
                    {
                        blockedFields[x, y] = false;
                    }
                }
            }
        }

    }
}
