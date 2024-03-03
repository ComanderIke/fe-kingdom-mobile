using Game.GameMechanics;
using Game.GUI.ToolTips;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class UIBlessingButton : MonoBehaviour
    {
        private Blessing blessing;
        [SerializeField] private Image image;
        
        public void SetValues(Blessing blessing)
        {
            this.blessing = blessing;
            image.sprite = this.blessing.Icon;
        }

        public void Clicked()
        {
            ToolTipSystem.Show(blessing, transform.position);
        }
    }
}
