using TMPro;
using UnityEngine;

namespace Game.GUI.PopUpText
{
    public class FloatingText : MonoBehaviour
    {
        public Animator Animator;
        [SerializeField] private readonly TextMeshProUGUI damageText;

        private void Start()
        {
            var clipInfo = Animator.GetCurrentAnimatorClipInfo(0);
            Destroy(gameObject, clipInfo[0].clip.length);
        }

        public void SetText(string text)
        {
            damageText.text = text;
        }
    }
}