using Assets.Scripts.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class GridBuilder
{
    const string CELL_NAME = "Grid Cell";
    const string CELL_TAG = "Grid";
    const int CELL_LAYER = 0;
    const string BLOCK_FIELD_LAYER = "BlockField";

    public float cellSize = 1;
    private int width;
    private int height;
    public bool[,] blockedFields;
    Transform gridTransform;
    Material cellMaterialStandard;
    Material cellMaterialInvalid;
    Tile[,] Tiles;

    public GridBuilder()
    {
       
        cellMaterialInvalid = GameObject.FindObjectOfType<RessourceScript>().materials.cellMaterialInValid;
        cellMaterialStandard = GameObject.FindObjectOfType<RessourceScript>().materials.cellMaterialValid;
        
        
    }

    public Tile[,] Build(int width, int height, Transform gridTransform)
    {
        this.width = width;
        this.height = height;
        this.gridTransform = gridTransform;
        Tiles = new Tile[width, height];
        foreach (Transform t in gridTransform.GetComponentsInChildren<Transform>())
        {
            if(t.gameObject != gridTransform.gameObject)
                GameObject.DestroyImmediate(t.gameObject);
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Tiles[i, j] = new Tile(i, j, CreateChild(i, j));

            }
        }
        UpdateCells();
        return Tiles;
    }

    GameObject CreateChild(int xvalue, float yvalue)
    {
        GameObject go = new GameObject();
        go.layer = CELL_LAYER;
        go.tag = CELL_TAG;
        go.name = CELL_NAME + " " + xvalue + " " + yvalue;
        go.transform.parent = gridTransform.transform;
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
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cell = Tiles[x, z].gameObject;
                MeshRenderer meshRenderer = cell.GetComponent<MeshRenderer>();
                MeshFilter meshFilter = cell.GetComponent<MeshFilter>();

                meshRenderer.material = blockedFields[x, z] ? cellMaterialInvalid : cellMaterialStandard;
                if (blockedFields[x, z])
                {
                    Tiles[x, z].isAccessible = false;
                }
                UpdateMesh(meshFilter.sharedMesh, x, z);
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
        blockedFields = new bool[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                origin = new Vector3(x * cellSize + (cellSize / 2), y * cellSize + (cellSize / 2), -5);
                if (Physics.Raycast(gridTransform.TransformPoint(origin), Vector3.forward, out hitInfo, Mathf.Infinity, LayerMask.GetMask(BLOCK_FIELD_LAYER)))
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

