using Game.DataAndReferences.References;
using Game.GameActors.Units;
using Game.Utility;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class PowerTriangleUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private MMF_Player effectiveFeedbacks;
        // [SerializeField] private MMF_Player inEffectiveFeedbacks;
        private bool effective = false;
        public void Set(PowerTriangleType type, bool effective)
        {
            icon.sprite= GameAssets.Instance.visuals.Icons.GetPowerTriangleIcon(type);
            this.effective = effective;
            MonoUtility.InvokeNextFrame(() =>
            {
                if(this.effective)
                    effectiveFeedbacks.PlayFeedbacks();
                else
                {
                    effectiveFeedbacks.StopFeedbacks();
                }
            });
          
        }
       
    }
}