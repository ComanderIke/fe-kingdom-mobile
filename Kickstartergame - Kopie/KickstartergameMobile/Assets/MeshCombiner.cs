using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour {

	// Use this for initialization
	public void CombineMeshes () {
        Quaternion oldRot = transform.rotation;
        Vector3 oldPos = transform.position;

        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;

        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();

        Mesh finalMesh = new Mesh();
        CombineInstance[] combiners = new CombineInstance[filters.Length];

        for(int i=0; i < filters.Length; i++)
        {
            if(filters[i].transform == transform)
            {
                continue;
            }
            combiners[i].subMeshIndex = 0;
            combiners[i].mesh = filters[i].sharedMesh;
            combiners[i].transform = filters[i].transform.localToWorldMatrix;
        }
        finalMesh.CombineMeshes(combiners);
        Debug.Log("Combined");
        finalMesh.name = "Test";
        GetComponent<MeshFilter>().sharedMesh = finalMesh;
        transform.rotation = oldRot;
        transform.position = oldPos;
        for(int i=0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
