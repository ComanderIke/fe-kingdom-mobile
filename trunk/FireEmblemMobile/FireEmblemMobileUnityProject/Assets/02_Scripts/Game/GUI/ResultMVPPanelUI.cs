using Game.GameActors.Units;
using Game.GUI.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class ResultMVPPanelUI : MonoBehaviour
    {

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI mvpName;
        [SerializeField] private Image mvpFaceSprite;
        [SerializeField] private float minTimeShown = 1.0f;
        private bool shown = false;
        private float time = 0;

        public void Show(Unit mvp)
        {
            gameObject.SetActive(true);
            mvpName.text = mvp.Name;
            mvpFaceSprite.sprite= mvp.visuals.CharacterSpriteSet.FaceSprite;
            TweenUtility.FadeIn(canvasGroup);
            shown = true;
            time = 0;
        }

        private void Update()
        {
            if (shown)
            {
                time += Time.deltaTime;
                if (time >= minTimeShown)
                {
                    if(Input.GetMouseButtonDown(0))
                        Hide();
                }
            }
        }

        public void Hide()
        {
            shown = false;
            TweenUtility.FadeOut(canvasGroup).setOnComplete(() => gameObject.SetActive(false));
        }
    }
}