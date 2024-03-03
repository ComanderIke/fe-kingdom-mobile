using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class UIHiddenButtonController : MonoBehaviour
    {
        [SerializeField]private Button activateOtherButton;
        void Start()
        {
        
        }

        void Update()
        {
        
        }

        public void Test()
        {
            Debug.Log("TEST WORKED");
        }

        public void Clicked()
        {
            ExecuteEvents.Execute(activateOtherButton.gameObject, new BaseEventData(EventSystem.current),
                ExecuteEvents.submitHandler);
        }
    }
}
