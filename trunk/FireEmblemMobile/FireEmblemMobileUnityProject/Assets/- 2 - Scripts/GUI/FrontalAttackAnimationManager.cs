using System.Collections;
using UnityEngine;

namespace Assets.GUI
{
    public class FrontalAttackAnimationManager : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(DisableObject(GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length * 2f));
        }

        private IEnumerator DisableObject(float delay)
        {
            yield return new WaitForSeconds(delay);
            UiSystem.OnFrontalAttackAnimationEnd();
            gameObject.SetActive(false);
        }
    }
}