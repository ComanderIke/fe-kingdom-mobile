using UnityEngine;

public class StartPosition : MonoBehaviour {

    // Use this for initialization
    void Start () {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
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
