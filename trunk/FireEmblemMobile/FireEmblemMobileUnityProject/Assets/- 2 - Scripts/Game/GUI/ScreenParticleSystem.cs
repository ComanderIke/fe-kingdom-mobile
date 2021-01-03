using UnityEngine;
using Utility;

namespace Game.GUI
{
    public class ScreenParticleSystem : MonoBehaviour
    {
        [SerializeField] private GameObject psTest = default;
        [SerializeField] private Camera psCamera = default;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var position = psCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
                //position.z = 1;
                var ps = Instantiate(psTest, position, Quaternion.identity);
                ps.layer = LayerMask.NameToLayer("FrontUI");
                ps.AddComponent<ParticleSystemAutoDestroy>();
                foreach (var trans in ps.transform.GetComponentsInChildren<Transform>())
                {
                    trans.gameObject.layer = LayerMask.NameToLayer("FrontUI");
                }
                ps.GetComponent<ParticleSystem>().Play(true);
            }
        }
    }
}