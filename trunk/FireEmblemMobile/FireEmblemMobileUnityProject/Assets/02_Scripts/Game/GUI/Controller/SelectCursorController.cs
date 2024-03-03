using UnityEngine;

namespace Game.GUI.Controller
{
    [ExecuteInEditMode]
    public class SelectCursorController : MonoBehaviour
    {
        public static SelectCursorController Instance;
        public GameObject selected;

        void Start()
        {
            Instance = this;
        }
        public void Show(GameObject selectedMetaSkill)
        {
            gameObject.SetActive(true);
            selected = selectedMetaSkill;
            transform.position = selected.transform.position;
        }

        private void OnEnable()
        {
            if (selected != null)
            {
                transform.position = selected.transform.position;
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}