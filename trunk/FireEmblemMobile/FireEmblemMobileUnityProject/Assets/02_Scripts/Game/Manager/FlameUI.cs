using Game.GameActors.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.Manager
{
    public class FlameUI : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject clickHintObject;
        [SerializeField] private GameObject feedTextPrefab;
        [SerializeField] private GameObject vfx;
        [SerializeField] private Slider flameExpSlider;
        [SerializeField] private TextMeshProUGUI flameLevelText;

        private void Awake()
        {
            Player.Instance.FlameExpChanged += FlameExpChanged;
            Player.Instance.FlameLevelChanged += FlameLevelChanged;
            Hide();
        }

        private void OnDestroy()
        {
            Player.Instance.FlameExpChanged -= FlameExpChanged;
            Player.Instance.FlameLevelChanged -= FlameLevelChanged;
        }

        void FlameExpChanged(float exp)
        {
            flameExpSlider.value = exp / 100f;
        }

        void FlameLevelChanged(int level)
        {
            flameLevelText.text = "Lv " + level;
        }
        public void Show()
        {
            gameObject.SetActive(true);
            clickHintObject.gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ExitClicked()
        {
            MyDebug.LogInput("Exit Flame Clicked");
            Hide();
        }
        public void Clicked()
        {
           
            clickHintObject.gameObject.SetActive(false);
            MyDebug.LogInput("Flame Clicked "+ Input.mousePosition);
           
            Vector2 pos;
            var go = Instantiate(vfx, transform);
           
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,Input.mousePosition,Camera.main, out pos);
            
            Vector2 randomOffset = new Vector2(Random.value*25*(Random.value>=.5f?1:-1), Random.value*10*(Random.value>=.5f?1:-1));
            go.transform.position = canvas.transform.TransformPoint(pos+randomOffset);
            
            if (Player.Instance.Grace < 10)
            {
                return;
            }
            var text = Instantiate(feedTextPrefab, transform);
            Player.Instance.Grace -= 10;
            Player.Instance.AddFlameExp(10);
            text.GetComponent<RectTransform>().position = canvas.transform.TransformPoint(pos+randomOffset);
            
            LeanTween.scale(text, new Vector3(1.5f, 1.5f, 1.5f), .4f).setEaseOutBack();
            LeanTween.moveY(text, text.transform.position.y + 5, 1.4f).setEaseInOutQuad().setOnComplete(
                ()=>LeanTween.alphaCanvas(text.GetComponent<CanvasGroup>(), 0,.9f).setEaseInOutQuad().setOnComplete(()=>GameObject.Destroy(text)));
        }
    }
}