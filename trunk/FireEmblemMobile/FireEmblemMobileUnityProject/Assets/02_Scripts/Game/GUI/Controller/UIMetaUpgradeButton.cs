using UnityEngine;
using UnityEngine.UI;

public class UIMetaUpgradeButton : MonoBehaviour
{
    public Image icon;
    private MetaUpgradeController metaUpgradeController;
    public MetaUpgradeBP metaUpgradeBp;
    private Image selected;

    public void SetValues(MetaUpgradeBP upgrade, MetaUpgradeController controller)
    {
        this.metaUpgradeBp = upgrade;
        this.metaUpgradeController = controller;
        icon.sprite = upgrade.icon;
    }
    public void Clicked()
    {
        metaUpgradeController.UpgradeClicked(this);
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