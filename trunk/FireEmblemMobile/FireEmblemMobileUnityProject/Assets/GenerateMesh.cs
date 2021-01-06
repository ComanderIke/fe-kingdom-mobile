using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class GenerateMesh : MonoBehaviour
{
    // Start is called before the first frame update
    public int xSize;
    public int ySize;
    private Vector3[] vertices;
    private Mesh mesh;
    private bool[][] grid = {new[]{false, false, false, true, true}, new[]{false, true, false, true, false},new[]{true, true, true, true, false}};
    void OnEnable()
    {
        //GenerateGrid();
        GenerateIncompleteGrid();
    }
    
    private void GenerateIncompleteGrid()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Incomplete Grid";
        int quadAmount = grid.Sum(t => t.Count(a => a));
        vertices = new Vector3[quadAmount*4];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        xSize = grid.Length;
        ySize = grid[0].Length;
        for (int i = 0, y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                if (grid[x][y])
                {
                    vertices[i] = new Vector3(x-0.5f, y-0.5f);
                    vertices[i+1] = new Vector3(x-0.5f, y+0.5f);
                    vertices[i+2] = new Vector3(x+0.5f, y-0.5f);
                    vertices[i+3] = new Vector3(x+0.5f, y+0.5f);
                    
                    uv[i] = new Vector2((x) / (float) xSize, (y) / (float) ySize);
                    uv[i+1] = new Vector2((x) / (float) xSize, (y+1) / (float) ySize);
                    uv[i+2] = new Vector2((x +1)/ (float) xSize, (y )/ (float) ySize);
                    uv[i+3] = new Vector2((x+1 )/ (float) xSize, (y+1) / (float) ySize);
                    tangents[i] = tangent;
                    tangents[i+1] = tangent;
                    tangents[i+2] = tangent;
                    tangents[i+3] = tangent;
                    i+=4;
                }
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;
        int[] triangles = new int[quadAmount*6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++)
        {
            for (int x = 0; x <xSize; x++)
            {
                if (grid[x][y])
                {
                    triangles[ti] = vi;
                    triangles[ti + 1] = triangles[ti + 3] = vi + 1;
                    triangles[ti + 2] = triangles[ti + 5] = vi + 2;
                    triangles[ti + 4] = vi + 3;
                    ti += 6;
                    vi+=4;
                }
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
    private void GenerateGrid()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2(x / (float) xSize, y / (float) ySize);
                tangents[i] = tangent;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;
        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 1] = triangles[ti + 3] = vi + 1;
                triangles[ti + 2] = triangles[ti + 5] = vi + xSize + 1;
                triangles[ti + 4] = vi + xSize + 2;
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        //var mesh = new Mesh {vertices = CreateQuad(0, 0)};
        //GetComponent<MeshFilter>().mesh = mesh;
    }
    /*private void OnDrawGizmos () {
        if (vertices == null) {
            return;
        }
        Gizmos.color = Color.black;
        
        for (int i = 0; i < vertices.Length; i++) {
            Gizmos.DrawSphere(transform.TransformPoint(vertices[i]), 0.1f);
        }
    }*/

   
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
