using UnityEngine;

namespace Game.Utility
{
    public class ParticleSystemAutoDestroy : MonoBehaviour {

        private ParticleSystem ps;

        public void Start()
        {
            ps = GetComponent<ParticleSystem>();
            if(ps==null)
                ps = GetComponentInChildren<ParticleSystem>();
            var main = ps.main;
            main.loop = false;
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
}
