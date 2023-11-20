using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LostGrace
{
    public class NPCFaceController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI speechText;

        public void Show(string text)
        {
            speechText.SetText(text);
        }
    }
}
