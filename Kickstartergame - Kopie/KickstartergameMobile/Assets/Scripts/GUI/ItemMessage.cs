using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemMessage : MonoBehaviour {

    private bool shown = false;
    private float time;
    private string itemName;
    public void Show(Item i)
    {
        shown = true;
        time = 0;
        itemName = i.Name;
        GetComponent<AudioSource>().Play();
        gameObject.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        gameObject.GetComponentInChildren<Text>().text = itemName + " used!";
    }
    void Update()
    {
        if (shown)
        {
            time += Time.deltaTime;
        }
        if (time > 1.7f)
        {
            time = 0;
            shown = false;
            gameObject.transform.position = new Vector3(-500, Screen.height / 2, 0);
        }
    }
}
