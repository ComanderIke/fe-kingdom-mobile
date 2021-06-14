using System;
using Game.GameActors.Units.Attributes;
using TMPro;
using UnityEngine;

namespace Game.GUI
{
    [Serializable]
    public class UIStatPanel :MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI strValueText;
        [SerializeField]
        private TextMeshProUGUI magValueText;
        [SerializeField]
        private TextMeshProUGUI spdValueText;
        [SerializeField]
        private TextMeshProUGUI sklValueText;
        [SerializeField]
        private TextMeshProUGUI defValueText;
        [SerializeField]
        private TextMeshProUGUI resValueText;
        public void SetStats(Stats stats)
        {
            strValueText.SetText(""+stats.Str);
            magValueText.SetText(""+stats.Mag);
            spdValueText.SetText(""+stats.Spd);
            sklValueText.SetText(""+stats.Skl);
            defValueText.SetText(""+stats.Def);
            resValueText.SetText(""+stats.Res);
        }
    }
}