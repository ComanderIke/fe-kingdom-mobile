using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCameraController : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera camera;
    public float speed;
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            var position = transform.position;
            position = new Vector3(position.x - Time.deltaTime * speed, position.y,
                position.z);
            transform.position = position;
        }
        if (Input.GetKey(KeyCode.D))
        {
            var position = transform.position;
            position = new Vector3(position.x + Time.deltaTime * speed, position.y,
                position.z);
            transform.position = position;
        }
    }
}
