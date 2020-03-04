using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI
{
    public class PlayerTurnTextAnimation : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public Image BackGround;
        private const float FADE_IN_DURATION = 0.6f;
        private const float FADE_OUT_DURATION = 0.4f;
        private const float OPAQUE_DURATION = 0.3f;
        private float backGroundMaxAlpha;
        public static float Duration = FADE_IN_DURATION + OPAQUE_DURATION + FADE_OUT_DURATION;

        private void Start()
        {
            backGroundMaxAlpha = BackGround.color.a;
            StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            float delay = FADE_IN_DURATION;
            float alpha = 0;
            while (delay > 0)
            {
                Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, alpha);
                BackGround.color = new Color(BackGround.color.r, BackGround.color.g, BackGround.color.b,
                    Mathf.Clamp(alpha, 0, backGroundMaxAlpha));
                alpha += 0.01f / FADE_IN_DURATION;
                delay -= 0.01f;
                yield return new WaitForSeconds(0.01f);
            }

            Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, 1);
            BackGround.color =
                new Color(BackGround.color.r, BackGround.color.g, BackGround.color.b, backGroundMaxAlpha);
            yield return new WaitForSeconds(OPAQUE_DURATION);
            delay = FADE_OUT_DURATION;
            alpha = 1;
            //gameObject.transform.parent.GetComponent<TransitionBase>().TransitionOut();
            while (delay > 0)
            {
                Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, alpha);
                BackGround.color = new Color(BackGround.color.r, BackGround.color.g, BackGround.color.b,
                    Mathf.Clamp(alpha, 0, backGroundMaxAlpha));
                alpha -= 0.01f / FADE_OUT_DURATION;
                delay -= 0.01f;
                yield return new WaitForSeconds(0.01f);
            }

            Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, 0);
            BackGround.color = new Color(BackGround.color.r, BackGround.color.g, BackGround.color.b, 0);
            Destroy(transform.parent.gameObject);
        }
    }
}