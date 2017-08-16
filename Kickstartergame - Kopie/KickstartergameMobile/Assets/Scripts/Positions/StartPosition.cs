using UnityEngine;
using System.Collections;
using Assets.Scripts.Characters.Classes;

public class StartPosition : MonoBehaviour {

    // Use this for initialization
    void Start () {
        GetComponentInChildren<MeshRenderer>().enabled = false;
	}
    public int GetX()
    {
        return (int)transform.localPosition.x;
    }
    public int GetY()
    {
        return (int)transform.localPosition.y;
    }

}
