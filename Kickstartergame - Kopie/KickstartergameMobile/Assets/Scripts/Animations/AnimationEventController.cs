using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class AnimationEventController : MonoBehaviour {

    [SerializeField]
    GameObject slashAnimation;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnAttackHit(){
        if (slashAnimation != null)
        {
            slashAnimation.SetActive(true);
            slashAnimation.GetComponent<ParticleSystem>().Play(true);
        }
        GameObject.Find ("Game").GetComponent<MainScript> ().AttackAnimationEvent ();
	}
    public void AttackSlashAnimation()
    {
        
    }
	public void DodgeEvent(){
		GameObject.Find ("Game").GetComponent<MainScript> ().DodgeEvent ();
	}
    public void OnDeathEvent()
    {
    }
}
