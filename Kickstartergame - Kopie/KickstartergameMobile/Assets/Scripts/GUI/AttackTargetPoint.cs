using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackTargetPoint : MonoBehaviour {

    public Animator animator;
    public int ID { get; set; }
    private Text missedText;
    private Text damageText;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        missedText = GetComponentsInChildren<Text>()[1];
        damageText = GetComponentsInChildren<Text>()[0];
        missedText.gameObject.SetActive(false);
        damageText.gameObject.SetActive(false);
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
        GetComponent<Image>().color = Color.white;
        animator.Play("Pressed");
    }
    public void StopAnimation()
    {
        animator.enabled = false;
        GetComponent<Image>().color = Color.black;
    }
}
