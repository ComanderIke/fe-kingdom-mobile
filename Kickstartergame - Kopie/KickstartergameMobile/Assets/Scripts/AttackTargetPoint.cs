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
    public void ShowMissedText()
    {
        StartCoroutine(TextAnimation(missedText));
    }
    public void ShowDamageText(int damage)
    {
        StartCoroutine(TextAnimation(damageText));
        damageText.text = "-" + damage;
    }
    IEnumerator TextAnimation(Text text)
    {
        float alpha = 0;
        text.gameObject.SetActive(true);
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        while (alpha < 1)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            alpha += AttackUIController.MISS_TEXT_FADE_IN_SPEED;
            yield return new WaitForSeconds(0.01f);
        }
        alpha = 1;
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        yield return new WaitForSeconds(AttackUIController.MISS_TEXT_VISIBLE_DURATION);
        while (alpha > 0)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            alpha -= AttackUIController.MISS_TEXT_FADE_OUT_SPEED;
            yield return new WaitForSeconds(0.01f);
        }
        alpha = 0;
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        text.gameObject.SetActive(false);

    }
}
