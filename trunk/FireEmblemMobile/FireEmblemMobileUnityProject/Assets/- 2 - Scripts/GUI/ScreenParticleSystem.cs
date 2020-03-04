using Assets.Utility;
using UnityEngine;

namespace Assets.GUI
{
    public class ScreenParticleSystem : MonoBehaviour
    {
        [SerializeField] private readonly GameObject psTest = default;
        [SerializeField] private readonly Camera psCamera = default;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var position = psCamera.ScreenToWorldPoint(Input.mousePosition);
                var ps = Instantiate(psTest, position, Quaternion.identity);
                ps.layer = LayerMask.NameToLayer("FrontUI");
                ps.AddComponent<ParticleSystemAutoDestroy>();
                foreach (var trans in ps.transform.GetComponentsInChildren<Transform>())
                {
                    trans.gameObject.layer = LayerMask.NameToLayer("FrontUI");
                }
            }
        }
    }
}