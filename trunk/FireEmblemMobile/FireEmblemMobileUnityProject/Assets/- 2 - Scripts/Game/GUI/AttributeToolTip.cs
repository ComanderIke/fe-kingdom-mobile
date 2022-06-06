using TMPro;
using UnityEngine;

public class AttributeToolTip : MonoBehaviour
{
    public TextMeshProUGUI header;
    public TextMeshProUGUI description;
    public TextMeshProUGUI value;
    public void SetValues(string header, string description, int value, Vector3 position)
    {
        this.header.text = header;
        this.description.text = description;
        this.value.text = ""+value;
        transform.position = position+ new Vector3(0,100,0);
    }
}