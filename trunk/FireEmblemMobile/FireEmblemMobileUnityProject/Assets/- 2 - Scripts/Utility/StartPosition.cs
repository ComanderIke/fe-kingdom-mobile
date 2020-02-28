using UnityEngine;

public class StartPosition : MonoBehaviour {

    // Use this for initialization
    void Start () {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
	}
    public int GetXOnGrid()
    {
        return (int)transform.localPosition.x;
    }
    public int GetYOnGrid()
    {
        return (int)transform.localPosition.y;
    }

}
