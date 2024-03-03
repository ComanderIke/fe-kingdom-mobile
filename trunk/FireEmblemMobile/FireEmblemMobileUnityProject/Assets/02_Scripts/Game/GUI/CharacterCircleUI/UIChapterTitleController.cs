using Game.EncounterAreas.AreaConstruction;
using Game.EncounterAreas.Management;
using TMPro;
using UnityEngine;

namespace Game.GUI.CharacterCircleUI
{
    public class UIChapterTitleController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeInTime;
        [SerializeField] private float stayDuration;
        [SerializeField] private float fadeOutTime;
        [SerializeField] private TextMeshProUGUI areaLabel;
        [SerializeField] private TextMeshProUGUI areaIndexText;
        void Start()
        {
            AreaGameManager.OnAreaStarted -= Show;
            AreaGameManager.OnAreaStarted += Show;
           

        }
        private void OnDestroy()
        {
            AreaGameManager.OnAreaStarted -= Show;
        }
        void Show(AreaData areaData)
        {
            this.areaIndexText.SetText(areaData.Index.ToString());
            string tagBegin = "";
            string tagEnd = "";
            foreach (var tag in areaData.textAnimatorTags)
            {
                tagBegin += "<" + tag + ">";
            }
            foreach (var tag in areaData.textAnimatorTags)
            {
                tagEnd += "</" + tag + ">";
            }
            this.areaLabel.SetText(tagBegin+areaData.Label+tagEnd);
            this.areaLabel.colorGradientPreset = areaData.ColorGradient;
           
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = true;
            LeanTween.alphaCanvas(canvasGroup, 1, fadeInTime).setEaseInOutQuad().setOnComplete(() =>
                LeanTween.alphaCanvas(canvasGroup, 0, fadeOutTime).setEaseInOutQuad().setDelay(stayDuration)
                    .setOnComplete(() => canvasGroup.blocksRaycasts = false));
        }
        

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
