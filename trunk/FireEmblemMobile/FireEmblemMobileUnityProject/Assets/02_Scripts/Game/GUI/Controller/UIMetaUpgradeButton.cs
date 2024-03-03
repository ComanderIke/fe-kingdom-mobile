using Game.MetaProgression;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.Controller
{
    public class UIMetaUpgradeButton : MonoBehaviour
    {
        public Image icon;
        private MetaUpgradeController metaUpgradeController;
        public MetaUpgradeBP metaUpgradeBp;
        public Image selected;
        public TextMeshProUGUI levelText;
        public Image levelTextBackground;
        public Sprite levelTextBackgroundGreen;
        public Sprite levelTextBackgroundRed;
        public TMP_ColorGradient maxedGradient;

        public void SetValues(MetaUpgradeBP upgrade, int level, bool affordable, MetaUpgradeController controller)
        {
            this.metaUpgradeBp = upgrade;
            this.metaUpgradeController = controller;
            icon.sprite = upgrade.icon;
            levelText.text = level+"/" + upgrade.maxLevel;
            if (level >= upgrade.maxLevel)
                levelText.colorGradientPreset = maxedGradient;
            else
            {
                levelText.colorGradientPreset = null;
            }
            levelTextBackground.sprite = affordable ? levelTextBackgroundGreen : levelTextBackgroundRed;
        }
        public void Clicked()
        {
            metaUpgradeController.Clicked(this);
        }

        public void Deselect()
        {
            selected.gameObject.SetActive(false);
        }
        public void Select()
        {
            selected.gameObject.SetActive(true);
        }
    }
}