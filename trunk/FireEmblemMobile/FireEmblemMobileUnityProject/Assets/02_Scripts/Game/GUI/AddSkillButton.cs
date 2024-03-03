using TMPro;
using UnityEngine;

namespace Game.GUI
{
    public class AddSkillButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI skillpointsText;
        [SerializeField] private GameObject skillPointsPreview;

        public void Show(int points)
        {
            if (points > 0)
            {
                skillpointsText.SetText(""+points);
                skillPointsPreview.gameObject.SetActive(true);
            }
            else
                skillPointsPreview.gameObject.SetActive(false);
        }

    }
}