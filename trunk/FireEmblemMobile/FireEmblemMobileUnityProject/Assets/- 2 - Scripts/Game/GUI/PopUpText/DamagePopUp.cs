using Assets.GameResources;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    public static DamagePopUp Create(Vector3 position, int damageAmount, Color color)
    {
        GameObject damagePopUpTransform = Instantiate(ResourceScript.Instance.Prefabs.DamagePopUptext, position, Quaternion.identity);

        DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
        damagePopUp.Setup(damageAmount, color);
        return damagePopUp;
    }

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private const float DISAPPEAR_TIMER_MAX = .7f;
    private Vector3 moveVector;
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }
    public void Setup(int damage, Color color)
    {
        textMesh.SetText(damage.ToString());
        disappearTimer = DISAPPEAR_TIMER_MAX;
        textMesh.color = color;

        //moveVector = new Vector3(.7f, 1) * 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += moveVector * Time.deltaTime;
        //moveVector -= moveVector * 8f * Time.deltaTime;
        if (disappearTimer> DISAPPEAR_TIMER_MAX * .5f)
        {
            float increaseAmount = .5f;
            transform.localScale += Vector3.one * increaseAmount * Time.deltaTime;

        }
        else
        {
            float decreaseAmount = .5f;
            transform.localScale -= Vector3.one * decreaseAmount * Time.deltaTime;
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
