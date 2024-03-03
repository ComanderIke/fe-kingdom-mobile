using UnityEngine;

namespace Game.Utility.UI
{
    public class UIInteractionBlockerOnEnable : MonoBehaviour
    {
      private void OnEnable()
        {
            InteractionBlocker.Instance.SetActive(gameObject,true);
        }
        private void OnDisable()
        {
            InteractionBlocker.Instance.SetActive(gameObject,false);
        }

       
    }
}