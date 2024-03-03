using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class UIWhiteRadialFillBar : MonoBehaviour
    {
        [SerializeField] private Image radialFill;
        [SerializeField] private Transform edgeTransform;

       // private float currentFill;
        public void AddFill(float fill)
        {
            
            radialFill.fillAmount += fill;
            if (radialFill.fillAmount <= 0)
                radialFill.fillAmount = 0;
            edgeTransform.transform.localRotation = Quaternion.Euler(0,0,radialFill.fillAmount*360);
        }

        public void SetFill(float specialFillAmount)
        {
           
            radialFill.fillAmount = specialFillAmount;
            edgeTransform.transform.localRotation = Quaternion.Euler(0,0,radialFill.fillAmount*360);
        }
    }
}
