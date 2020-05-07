using Assets.Core;
using UnityEngine;

namespace Assets.GUI.Base
{
    public class BottomBarController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void GoIntoBattleClicked()
        {
            SceneController.SwitchScene("Level2");
        }
    }
}
