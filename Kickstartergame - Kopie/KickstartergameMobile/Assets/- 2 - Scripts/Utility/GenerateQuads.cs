using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GenerateQuads : MonoBehaviour
{

    public float size = 1;
    public int gridSize = 4;
    private MeshFilter filter;
    public Mesh Mesh
    {
        get
        {
            return filter.mesh;
        }
    }
    // Use this for initialization
    void Start()
    {
       
    }
    public void GenerateMesh(List<Vector2> vectors)
    {
        filter = GetComponent<MeshFilter>();
        List<Mesh> meshes = new List<Mesh>();

        CombineInstance[] combineInstances = new CombineInstance[vectors.Count];
        int index = 0;
        foreach (Vector2 v in vectors)
        {
            combineInstances[index].mesh = GenerateQuad(v);
            combineInstances[index].transform = Matrix4x4.identity;
            meshes.Add(GenerateQuad(v));
            index++;
        }
        filter.mesh = new Mesh();
        filter.mesh.CombineMeshes(combineInstances);
    }

    // Update is called once per frame
    void Update()
    {

    }

  
    Mesh GenerateQuad(Vector2 position)
    {
        Mesh mesh = new Mesh();

        mesh.SetVertices(new List<Vector3>()
        {
            new Vector3(-size * 0.5f+position.x+0.5f,0, -size *0.5f+position.y+0.5f),
            new Vector3(size * 0.5f+position.x+0.5f,0, -size *0.5f+position.y+0.5f),
            new Vector3(size * 0.5f+position.x+0.5f,0, size *0.5f+position.y+0.5f),
            new Vector3(-size * 0.5f+position.x+0.5f,0, size *0.5f+position.y+0.5f)
        });
        mesh.SetTriangles(new List<int>()
        {
            3,1,0,
            3,2,1
        }, 0);
        mesh.SetNormals(new List<Vector3>()
        {
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up
        });
        mesh.SetUVs(0, new List<Vector2>()
        {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(1,1),
            new Vector2(0,1)
        });
        return mesh;
    }
}
