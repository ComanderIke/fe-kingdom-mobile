using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackTargetPoint : MonoBehaviour {

    public Animator animator;
    public Animator animator2;
    public int ID { get; set; }
    private Text missedText;
    private Text damageText;
    // Use this for initialization
    void Start () {
        missedText = GetComponentsInChildren<Text>()[1];
        damageText = GetComponentsInChildren<Text>()[0];
        missedText.gameObject.SetActive(false);
        damageText.gameObject.SetActive(false);
        //PlayAnimation();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnClick()
    {
        FindObjectOfType<AttackUIController>().TargetPointSelected(ID);
        
    }
    public void PlayAnimation()
    {
        animator.enabled = true;
        animator2.enabled = false;
        animator2.gameObject.SetActive(false);
        animator.gameObject.GetComponent<Image>().color = new Color(animator.gameObject.GetComponent<Image>().color.r, animator.gameObject.GetComponent<Image>().color.g, animator.gameObject.GetComponent<Image>().color.b, 1f);
        animator.Play("Pressed");
    }
    public void StopAnimation()
    {
        animator.enabled = false;
       
        animator2.enabled = false;
        animator2.gameObject.SetActive(false);
        animator.gameObject.GetComponent<Image>().color = new Color(animator.gameObject.GetComponent<Image>().color.r, animator.gameObject.GetComponent<Image>().color.g, animator.gameObject.GetComponent<Image>().color.b, 0.2f);
    }
}
