using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour {

    public Animator animator;
    [SerializeField]
    private TextMeshProUGUI damageText;

    void Start()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
    }
    public void SetText(string text)
    {
        damageText.text = text;
    }
}
