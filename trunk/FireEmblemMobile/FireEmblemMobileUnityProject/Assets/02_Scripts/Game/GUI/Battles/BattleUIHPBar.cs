using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.Battles
{
    public class BattleUIHPBar : MonoBehaviour
    {
        public Image currentHPBar;

        public Image LosingHPBar;

        private int currentHp;

        private int maxHP;

   

        private const float UpdateSpeedSeconds = 0.2f;
        private const float UpdateSpeedLosingBarSeconds = 0.9f;
        // Start is called before the first frame update
        public void SetValues(int maxHp, int currentHp)
        {
            this.currentHp = currentHp;
            this.maxHP = maxHp;

            StartCoroutine(AnimateBar(0f,currentHp / (maxHp*1.0f)));
            StartCoroutine(AnimateBar2(0.35f, currentHp / (maxHp*1.0f)));
        }

        private IEnumerator AnimateBar(float delay,float pct)
        {
            yield return new WaitForSeconds(delay);
            float preChangePct = currentHPBar.fillAmount;
            float elapsed = 0;
            while (elapsed < UpdateSpeedSeconds)
            {
                elapsed += Time.deltaTime;
                currentHPBar.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / UpdateSpeedSeconds);
                yield return null;
            }

            currentHPBar.fillAmount = pct;
        }
        private IEnumerator AnimateBar2(float delay, float pct)
        {
            yield return new WaitForSeconds(delay);
            float preChangePct = LosingHPBar.fillAmount;
            float elapsed = 0;
            while (elapsed < UpdateSpeedLosingBarSeconds)
            {
                elapsed += Time.deltaTime;
                LosingHPBar.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / UpdateSpeedLosingBarSeconds);
                yield return null;
            }

            LosingHPBar.fillAmount = pct;
        }
   
    }
}
