using UnityEngine;
using System.Collections;

public class CursorScript : MonoBehaviour {

    private Vector2 position;
    public bool lightningcursor = false;
    public bool wallcursor = false;
    public Material defaultMaterial;
    public Material lightningMaterial;
    public Material wallMaterial;
    public Quaternion wallRotation;
    void Start () {}
   
	void Update () {
        //Debug.Log("CursorPos: " + transform.position);
        if (!MyInputManager.GAMEPAD_INPUT)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        // lightningcursor = true;
        if (wallcursor)
        {
            gameObject.GetComponent<MeshRenderer>().material = wallMaterial;
            gameObject.transform.localScale = new Vector3(3, 1, 1);
            gameObject.transform.localRotation = wallRotation;
            //gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 0, 0.1f);
        }
        else if (lightningcursor)
        {
            gameObject.GetComponent<MeshRenderer>().material = lightningMaterial;
            gameObject.transform.localScale = new Vector3(3, 3, 3);
            //gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 0, 0.1f);
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
           
        }
        if (MyInputManager.position != null&&MyInputManager.GAMEPAD_INPUT)
        {
            int x = (int)MyInputManager.position.transform.position.x;
            int y = (int)MyInputManager.position.transform.position.z;

            if (!GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().MoveCursorTo(x, y))
            {
                MyInputManager.fixedPosition = true;
            }
            else
            {
                MyInputManager.fixedPosition = false;
            }
        }

    }

    public void SetPosition(float x, float y, float z)
    {
        if (MyInputManager.position != null)
        {
            this.transform.localPosition = new Vector3(x, y, z);
            MyInputManager.position.transform.localPosition = new Vector3(x, 0, z);
        }
    }
}
