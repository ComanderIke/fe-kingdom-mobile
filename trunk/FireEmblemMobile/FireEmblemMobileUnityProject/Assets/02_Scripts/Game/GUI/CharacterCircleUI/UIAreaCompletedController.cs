using Game.EncounterAreas.Management;
using TMPro;
using UnityEngine;

namespace Game.GUI.CharacterCircleUI
{
    public class UIAreaCompletedController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeInTime=1.2f;
        [SerializeField] private float stayDuration=2f;
        [SerializeField] private float fadeOutTime=1f;
        [SerializeField] private TextMeshProUGUI areaIndex;
        // Start is called before the first frame update
        void Start()
        {
            AreaGameManager.OnAreaCompleted -= Show;
            AreaGameManager.OnAreaCompleted += Show;
        }

        private void OnDestroy()
        {
            AreaGameManager.OnAreaCompleted -= Show;
        }

        // Update is called once per frame
        void Show(int areaIndex)
        {
            this.areaIndex.SetText((areaIndex+1).ToString());
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = true;
            LeanTween.alphaCanvas(canvasGroup, 1, fadeInTime).setEaseInOutQuad().setOnComplete(() =>
                LeanTween.alphaCanvas(canvasGroup, 0, fadeOutTime).setEaseInOutQuad().setDelay(stayDuration));
        }
    }
}
