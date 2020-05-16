
using UnityEngine;

//[ExecuteInEditMode]
public class FogController : MonoBehaviour
{

    public float startPos;
    private float length;
    public float speed = 1;
    void Start()
    {
        length = transform.GetComponent<SpriteRenderer>().bounds.size.x;
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + Time.deltaTime * speed, transform.position.y, transform.position.z);
        if (startPos - transform.position.x > length / 2)
        {
            transform.position = new Vector3(transform.position.x + length, transform.position.y, transform.position.z);
        }
        else if (transform.position.x - startPos > length / 2)
        {
            transform.position = new Vector3(transform.position.x - length, transform.position.y, transform.position.z);
        }
            
       
    }

  
}
