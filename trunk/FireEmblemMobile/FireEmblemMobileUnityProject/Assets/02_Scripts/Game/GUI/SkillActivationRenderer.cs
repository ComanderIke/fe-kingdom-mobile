using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace LostGrace
{
    public class SkillActivationRenderer : MonoBehaviour
    {
        [SerializeField] private GameObject skillActivationPrefab;
        [SerializeField] private float delayBetweenSkills;
        [SerializeField] private Transform leftParent;
        [SerializeField] private Transform rightParent;
       
        public void Show(Unit activater, List<Skill> activatedSkills, bool left)
        {
            if(left)
                leftParent.DeleteChildren();
            else
            {
                rightParent.DeleteChildren();
            }
            StartCoroutine(SpawnSkillActivationUICoroutine(activater, activatedSkills, left));
          
        }
        public void Hide(List<Skill> activatedSkills)
        {
            leftParent.DeleteChildren();
            rightParent.DeleteChildren();
        }
        IEnumerator SpawnSkillActivationUICoroutine(Unit activater, List<Skill> activatedSkills, bool left)
        {
            foreach (var skill in activatedSkills)
            {
                var go = Instantiate(skillActivationPrefab, left?leftParent:rightParent);
                go.GetComponent<SkillActivatedUI>().SetSkill(activater, skill);
                yield return new WaitForSeconds(delayBetweenSkills);
            }
            
        }
    }

    
}
