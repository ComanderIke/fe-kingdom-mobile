using Game.GameResources;
using TMPro;
using UnityEngine;

namespace Game.GUI.PopUpText
{
    public class DamagePopUp : MonoBehaviour
    {
        public static DamagePopUp Create(Vector3 position, int damageAmount, Color color, float scale=1.0f)
        {
            GameObject damagePopUpTransform = Instantiate(GameAssets.Instance.prefabs.DamagePopUptext, position, Quaternion.identity);

            DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
            damagePopUp.Setup(damageAmount, color, scale);
            return damagePopUp;
        }

        private TextMeshPro textMesh;
        private float disappearTimer;
        private Color textColor;
        private const float DISAPPEAR_TIMER_MAX = .7f;
        private Vector3 moveVector;
        private Vector3 baseScale;
        private void Awake()
        {
            textMesh = transform.GetComponent<TextMeshPro>();
        }
        public void Setup(int damage, Color color, float scale)
        {
            textMesh.SetText(damage.ToString());
            disappearTimer = DISAPPEAR_TIMER_MAX;
            textMesh.color = color;
            baseScale = new Vector3(scale,scale,scale);
            moveVector = new Vector3(.2f, .7f) * 2.5f;
            transform.localScale = baseScale;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position += moveVector * Time.deltaTime;
            moveVector -= moveVector * 8f * Time.deltaTime;
            
            if (disappearTimer> DISAPPEAR_TIMER_MAX * .5f)
            {
                float increaseAmount = .5f;
                transform.localScale += baseScale* increaseAmount * Time.deltaTime;

            }
            else
            {
                float decreaseAmount = .5f;
                transform.localScale -= baseScale * decreaseAmount * Time.deltaTime;
            }
            disappearTimer -= Time.deltaTime;
            if(disappearTimer < 0)
            {
                float disapperSpeed = 3f;
                textColor.a -= disapperSpeed * Time.deltaTime;
                textMesh.color = textColor;
                if (textColor.a < 0)
                    Destroy(gameObject);
            }
        }
    }
}
