using Assets.Scripts.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontalAttackAnimationManager : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
        StartCoroutine(DisableObject(GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length*2f));
	}
	
    IEnumerator DisableObject(float delay)
    {
        yield return new WaitForSeconds(delay);
        EventContainer.frontalAttackAnimationEnd();
        gameObject.SetActive(false);
    }
}
