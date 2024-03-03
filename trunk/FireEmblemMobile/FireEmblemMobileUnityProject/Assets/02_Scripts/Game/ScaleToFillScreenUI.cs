using UnityEngine;

namespace Game
{
    public class ScaleToFillScreenUI : MonoBehaviour
    {
        // Start is called before the first frame update
        void OnEnable()
        {
            MyDebug.LogTest("Screen Width: "+Screen.width+" Height: "+Screen.height);
            GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
