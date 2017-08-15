using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClearSight : MonoBehaviour {

    // Use this for initialization//
    //http://answers.unity3d.com/questions/44815/make-object-transparent-when-between-camera-and-pl.html
    void Start () {
        positions = new List<Vector3>();
    }
    const string BLOCK_FIELD_LAYER = "BlockField";
    //public int layermask;
    public float DistanceToPlayer;
    List<Vector3> positions;
    void Update()
    {
        RaycastHit[] hits;
        // you can also use CapsuleCastAll()
        // TODO: setup your layermask it improve performance and filter your hits.
        //Debug.Log(LayerMask.GetMask(BLOCK_FIELD_LAYER));
        positions.Clear();
        for(int x=0; x <5; x++)
        {
            for(int z=0; z<5; z++)
            {
                positions.Add(new Vector3(transform.position.x+x, transform.position.y, transform.position.z+z));
                positions.Add(new Vector3(transform.position.x - x, transform.position.y, transform.position.z-z));
                positions.Add(new Vector3(transform.position.x - x, transform.position.y, transform.position.z+z));
                positions.Add(new Vector3(transform.position.x + x, transform.position.y, transform.position.z-z));
            }
        }
        foreach (Vector3 position in positions)
        {
            hits = Physics.RaycastAll(position, transform.forward, Mathf.Infinity, LayerMask.GetMask(BLOCK_FIELD_LAYER));
            foreach (RaycastHit hit in hits)
            {
                Renderer R = hit.collider.GetComponent<Renderer>();
                if (R == null)
                    continue; // no renderer attached? go to next hit
                              // TODO: maybe implement here a check for GOs that should not be affected like the player


                AutoTransparent AT = R.GetComponent<AutoTransparent>();
                if (AT == null) // if no script is attached, attach one
                {
                    AT = R.gameObject.AddComponent<AutoTransparent>();
                }
                AT.BeTransparent(); // get called every frame to reset the falloff
            }
        }
    }

}
