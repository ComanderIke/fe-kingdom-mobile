using LostGrace;
using UnityEngine;
using UnityEngine.UI;

public class BlessingTooltip : SkillToolTip
{
    [SerializeField] private Image blessingIcon;
    [SerializeField] private Image background;
    [SerializeField] private Image frame;
    public void SetValues(Blessing blessing, Vector3 position)
    {
        blessingIcon.sprite = blessing.Icon;
        background.material.SetColor("_TintColor", blessing.God.TooltipColor);
        frame.material.SetColor("_TintColor", blessing.God.TooltipFrameColor);
        base.SetValues(blessing, false,false, position);
    }

}