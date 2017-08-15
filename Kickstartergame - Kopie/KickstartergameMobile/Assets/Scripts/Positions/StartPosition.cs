using UnityEngine;
using System.Collections;
using Assets.Scripts.Characters.Classes;

public class StartPosition : MonoBehaviour {

    public CharacterClassType charType;
    // Use this for initialization
    void Start () {
        GetComponentInChildren<MeshRenderer>().enabled = false;
	}
    public int GetX()
    {
        return (int)transform.localPosition.x;
    }
    public int GetZ()
    {
        return (int)transform.localPosition.z;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
