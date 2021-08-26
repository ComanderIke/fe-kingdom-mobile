using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIHPBar : MonoBehaviour
{
    public Image currentHPBar;

    public Image LosingHPBar;

    private int currentHp;

    private int maxHP;

    private float elapsedTime = 0;

    private const float UpdateSpeedSeconds = 0.3f;
    private const float UpdateSpeedLosingBarSeconds = 1.2f;
    // Start is called before the first frame update
    public void SetValues(int maxHp, int currentHp)
    {
        this.currentHp = currentHp;
        this.maxHP = maxHp;
        elapsedTime = 0;
        StartCoroutine(AnimateBar(currentHp / (maxHp*1.0f)));
        StartCoroutine(AnimateBar2(currentHp / (maxHp*1.0f)));
    }

    private IEnumerator AnimateBar(float pct)
    {
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
    private IEnumerator AnimateBar2(float pct)
    {
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
