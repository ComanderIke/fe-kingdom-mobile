using TMPro;
using UnityEngine;

namespace LostGrace
{
    public class AddSkillButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI skillpointsText;
        [SerializeField] private GameObject skillPointsPreview;

        public void Show(int points)
        {
            if (points > 0)
            {
                skillpointsText.SetText(""+skillpointsText);
                skillPointsPreview.gameObject.SetActive(true);
            }
            else
                skillPointsPreview.gameObject.SetActive(false);
        }

    }
}