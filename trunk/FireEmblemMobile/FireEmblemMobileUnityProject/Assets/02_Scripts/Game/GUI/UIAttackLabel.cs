using TMPro;
using UnityEngine;

namespace LostGrace
{
    public class UIAttackLabel : MonoBehaviour
    {
        [SerializeField] private GameObject redArrowsRight;
        [SerializeField] private GameObject redArrowsLeft;
        public void SetValue(string text, bool showRightArrow)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = "<bounce>"+text;
            redArrowsRight.gameObject.SetActive(showRightArrow);
            redArrowsLeft.gameObject.SetActive(!showRightArrow);
            
        }
    }
}