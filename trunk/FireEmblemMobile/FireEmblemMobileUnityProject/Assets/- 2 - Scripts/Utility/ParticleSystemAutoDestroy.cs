using UnityEngine;
using System.Collections;

public class ParticleSystemAutoDestroy : MonoBehaviour {

    private ParticleSystem ps;


    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
        if(ps==null)
            ps = GetComponentInChildren<ParticleSystem>();
    }

    public void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
