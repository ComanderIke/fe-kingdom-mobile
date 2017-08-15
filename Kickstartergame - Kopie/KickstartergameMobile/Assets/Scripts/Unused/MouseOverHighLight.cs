using UnityEngine;
using System.Collections;

public class MouseOverHighLight : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnMouseEnter()
    {
        SkinnedMeshRenderer m = GetComponentInChildren<SkinnedMeshRenderer>();
        if (m == null)
            m = GetComponent<SkinnedMeshRenderer>();
        for (int i = 0; i < m.materials.Length; i++)
        {
            Color c = m.materials[i].color;
            m.materials[i].color = new Color(c.r+0.1f, c.g + 0.1f, c.b + 0.1f, c.a) ;
        }
    }
    void OnMouseExit()
    {
        SkinnedMeshRenderer m = GetComponentInChildren<SkinnedMeshRenderer>();
        if (m == null)
            m = GetComponent<SkinnedMeshRenderer>();
        for (int i = 0; i < m.materials.Length; i++)
        {
            Color c = m.materials[i].color;
            m.materials[i].color = new Color(c.r-0.1f, c.g - 0.1f, c.b - 0.1f, c.a);
        }

    }
}
