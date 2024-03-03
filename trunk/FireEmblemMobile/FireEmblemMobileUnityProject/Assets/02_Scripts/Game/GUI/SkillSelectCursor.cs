using UnityEngine;

namespace Game.GUI
{
    public class SkillSelectCursor : MonoBehaviour
    {
        public static SkillSelectCursor Instance;
        public GameObject selected;
        // Start is called before the first frame update
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
