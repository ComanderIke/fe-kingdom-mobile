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
            damagePopUp.Setup(damageAmount.ToString(), color, scale,   new Vector3(.2f, .7f) * 2.5f);
            return damagePopUp;
        }

        private TextMeshPro textMesh;
        private float disappearTimer;
        private Color textColor;
        private const float DISAPPEAR_TIMER_MAX = 0.6f;
        private Vector3 moveVector;
        private Vector3 baseScale;
        private void Awake()
        {
            textMesh = transform.GetComponent<TextMeshPro>();
        }
        public void Setup(string damage, Color color, float scale, Vector2 moveVector)
        {
            textMesh.SetText(damage.ToString());
            disappearTimer = DISAPPEAR_TIMER_MAX;
            textMesh.color = color;
            baseScale = new Vector3(scale,scale,scale);
            textColor = color;
            this.moveVector = moveVector;
            //Debug.Log("TextColorVefore: "+textColor);
            transform.localScale = baseScale;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position += moveVector * Time.deltaTime;
            moveVector -= moveVector * 1f * Time.deltaTime;
            
            // if (disappearTimer> DISAPPEAR_TIMER_MAX * .5f)
            // {
            //     float increaseAmount = .3f;
            //     transform.localScale += baseScale* increaseAmount * Time.deltaTime;
            //
            // }
            // else
            // {
                float decreaseAmount = .1f;
                transform.localScale -= baseScale * decreaseAmount * Time.deltaTime;
            // }
            disappearTimer -= Time.deltaTime;
            if(disappearTimer < 0)
            {
                float disapperSpeed = 2f;
                //Debug.Log("TextColor: "+textColor);
                textColor.a -= disapperSpeed * Time.deltaTime;
                textMesh.color = textColor;
              //  Debug.Log("TextColorAfter: "+textColor);
                if (textColor.a < 0)
                    Destroy(gameObject);
            }
        }

        public static DamagePopUp CreateForBattleView(Vector3 transformPosition, int dmg, Color color, float scale, Vector2 moveVector)
        {
            GameObject damagePopUpTransform = Instantiate(GameAssets.Instance.prefabs.DamagePopUptext, new Vector3 (transformPosition.x, transformPosition.y, transformPosition.z), Quaternion.identity);
            damagePopUpTransform.layer = LayerMask.NameToLayer("Characters");
            DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
            damagePopUp.Setup(dmg.ToString(), color, scale, moveVector);
            return damagePopUp;
        }

        public static DamagePopUp CreateMiss(Vector3 transformPosition, Color color, float scale, Vector2 moveVector)
        {
            GameObject damagePopUpTransform = Instantiate(GameAssets.Instance.prefabs.DamagePopUptext, new Vector3 (transformPosition.x, transformPosition.y, transformPosition.z), Quaternion.identity);
            damagePopUpTransform.layer = LayerMask.NameToLayer("Characters");
            DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
            damagePopUp.Setup("Missed!", color, scale, moveVector);
            return damagePopUp;
        }
    }
}
