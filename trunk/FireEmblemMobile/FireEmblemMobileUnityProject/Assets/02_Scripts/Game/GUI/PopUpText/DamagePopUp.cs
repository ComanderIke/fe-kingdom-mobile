using Game.GameResources;
using TMPro;
using UnityEngine;

namespace Game.GUI.PopUpText
{
    public enum TextStyle
    {
        Damage,Missed, Critical, NoDamage,
        Heal
    }

    public class DamagePopUp : MonoBehaviour
    {
        public TMP_ColorGradient missedColor;
        public TMP_ColorGradient damageColor;
        public TMP_ColorGradient noDamageColor;
        public TMP_ColorGradient criticalColor;
        public TMP_ColorGradient healColor;

        public static DamagePopUp Create(Vector3 position, int damageAmount, TextStyle style, float scale = 1.0f)
        {
            Debug.LogError("Should not be called?! Will this be called automatically when unit gets damaged?");
            GameObject damagePopUpTransform =
                Instantiate(GameAssets.Instance.prefabs.DamagePopUptext, position, Quaternion.identity);
            Debug.Log("");
            DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
            damagePopUp.Setup(damageAmount.ToString(), style, scale, new Vector3(.2f, .7f) * 2.5f);
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

        public void Setup(string damage,TextStyle style, float scale, Vector2 moveVector)
        {
            textMesh.SetText(damage.ToString());
            disappearTimer = DISAPPEAR_TIMER_MAX;
            switch (style)
            {
                case TextStyle.Critical:textMesh.colorGradientPreset = criticalColor;
                    break;
                case TextStyle.Missed:textMesh.colorGradientPreset = missedColor;
                    break;
                case TextStyle.Damage:textMesh.colorGradientPreset = damageColor;
                    break;
                case TextStyle.NoDamage:textMesh.colorGradientPreset = noDamageColor;
                    break;
                case TextStyle.Heal:textMesh.colorGradientPreset = healColor;
                    break;
                
                
            }
            baseScale = new Vector3(scale, scale, scale);
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
            if (disappearTimer < 0)
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

        public static DamagePopUp CreateForBattleView(Vector3 transformPosition, int dmg, TextStyle style, float scale,
            Vector2 moveVector)
        {

            GameObject damagePopUpTransform = Instantiate(GameAssets.Instance.prefabs.DamagePopUptext,
                new Vector3(transformPosition.x, transformPosition.y, transformPosition.z), Quaternion.identity);
            damagePopUpTransform.layer = LayerMask.NameToLayer("Characters");
            DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
            if(style == TextStyle.Critical)
                damagePopUp.Setup(dmg.ToString()+" Crit", style, scale, moveVector);
            else
                damagePopUp.Setup(dmg.ToString(), style, scale, moveVector);
            return damagePopUp;
        }

        public static DamagePopUp CreateMiss(Vector3 transformPosition, TextStyle style, float scale, Vector2 moveVector)
        {
            Debug.Log("MoveVector for Miss: " + moveVector);
            GameObject damagePopUpTransform = Instantiate(GameAssets.Instance.prefabs.DamagePopUptext,
                new Vector3(transformPosition.x, transformPosition.y, transformPosition.z), Quaternion.identity);
            damagePopUpTransform.layer = LayerMask.NameToLayer("Characters");
            DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
            damagePopUp.Setup("Missed!", style, scale, moveVector);
            return damagePopUp;
        }
    }
}