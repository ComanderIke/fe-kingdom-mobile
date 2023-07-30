using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units.Skills;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class SkillActivatedUI : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image icon;
        [SerializeField] private MMFeedbacks skillActivationFeedback;
        public void SetSkill(Skill skill)
        {
            this.nameText.text = skill.Name;
            icon.sprite = skill.GetIcon();
            MonoUtility.InvokeNextFrame(()=>skillActivationFeedback.PlayFeedbacks());
            
        }
    }
}
