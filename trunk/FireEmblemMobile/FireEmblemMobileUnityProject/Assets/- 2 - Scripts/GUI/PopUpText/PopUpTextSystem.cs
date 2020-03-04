using Assets.Core;
using UnityEngine;

namespace Assets.GUI.PopUpText
{
    public class PopUpTextSystem : MonoBehaviour, IEngineSystem
    {
        [SerializeField] private GameObject popUpTextRedPrefab = default;
        [SerializeField] private GameObject popUpTextBluePrefab = default;
        [SerializeField] private GameObject popUpTextGreenPrefab = default;
        [SerializeField] private Canvas attackCanvas = default;
        [SerializeField] private Canvas defendCanvas = default;

        public void CreateAttackPopUpTextRed(string text, Transform location)
        {
            var instance = Instantiate(popUpTextRedPrefab);
            instance.transform.SetParent(attackCanvas.transform, false);
            instance.transform.position = location.position;
            float randomX = Random.Range(-1.0f, 1.0f) * 0.5f;
            float randomY = Random.Range(-1.0f, 1.0f) * 0.5f;
            instance.transform.Translate(new Vector3(randomX, randomY, 0));
            instance.GetComponentInChildren<FloatingText>().SetText(text);
            //EZCameraShake.CameraShaker.Instance.ShakeOnce(5f, 10+1f * Int32.Parse(text), .1f, 1f);
        }

        public void CreateAttackPopUpTextBlue(string text, Transform location)
        {
            var instance = Instantiate(popUpTextBluePrefab);
            instance.transform.SetParent(attackCanvas.transform, false);
            instance.transform.position = location.position;
            float randomX = Random.Range(-1.0f, 1.0f) * 0.5f;
            float randomY = Random.Range(-1.0f, 1.0f) * 0.5f;
            instance.transform.Translate(new Vector3(randomX, randomY, 0));
            instance.GetComponentInChildren<FloatingText>().SetText(text);
            //EZCameraShake.CameraShaker.Instance.ShakeOnce(5f, 10 + 1f * Int32.Parse(text), .1f, 1f);
        }

        public void CreateAttackPopUpTextGreen(string text, Transform location)
        {
            var instance = Instantiate(popUpTextGreenPrefab);
            instance.transform.SetParent(attackCanvas.transform, false);
            instance.transform.position = location.position;
            instance.GetComponentInChildren<FloatingText>().SetText(text);
        }

        public void CreateDefendPopUpTextRed(string text, Transform location)
        {
            var instance = Instantiate(popUpTextRedPrefab);
            instance.transform.SetParent(defendCanvas.transform, false);
            instance.transform.position = location.position;
            float randomX = Random.Range(-1.0f, 1.0f) * 0.5f;
            float randomY = Random.Range(-1.0f, 1.0f) * 0.5f;
            instance.transform.Translate(new Vector3(randomX, randomY, 0));
            instance.GetComponentInChildren<FloatingText>().SetText(text);
            //EZCameraShake.CameraShaker.Instance.ShakeOnce(2f, 1f* Int32.Parse(text), .1f, 1f);
        }

        public void CreateDefendPopUpTextGreen(string text, Transform location)
        {
            var instance = Instantiate(popUpTextGreenPrefab);
            instance.transform.SetParent(defendCanvas.transform, false);
            instance.transform.position = location.position;
            instance.GetComponentInChildren<FloatingText>().SetText(text);
        }
    }
}