using TMPro;
using UnityEngine;

namespace Game.GUI.CharacterCircleUI
{
    public class ErrorScreenUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI errorText;
        // Start is called before the first frame update
        void Awake()
        {
            Application.logMessageReceived -= LogMessageReceived;
            Application.logMessageReceived += LogMessageReceived;
            Hide();
        }

        void LogMessageReceived(string condition, string stackTrace, LogType logType)
        {
            if (logType == LogType.Exception || logType == LogType.Error)
            {
                Show();
                errorText.text = "Error: " + condition + "\n" + stackTrace;
            }
        }

        void Show()
        {
            gameObject.SetActive(true);
            GetComponent<Canvas>().enabled = true;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= LogMessageReceived;
        }
    }
}
