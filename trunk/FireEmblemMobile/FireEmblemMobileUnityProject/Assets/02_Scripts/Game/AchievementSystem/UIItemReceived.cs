using System.Collections;
using Game.GameActors.Items;
using Game.GameActors.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the display of achievements on the screen
/// </summary>
///
///
public class UIItemReceived : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private Image icon;
    [SerializeField] private ItemStackUI itemStack;

    [SerializeField] private float displayTime = 2.2f;
    public void Set(Item item, ItemStackUI itemStackUI)
    {
        this.itemStack = itemStackUI;
        name.text = item.Name+" received";
        icon.sprite = item.Sprite;
    }

    public void StartDeathTimer ()
    {
        StartCoroutine(Wait());
    }
    private IEnumerator Wait ()
    {
        yield return new WaitForSeconds(displayTime);
        GetComponent<Animator>().SetTrigger("ScaleDown");
        yield return new WaitForSeconds(0.4f);
        itemStack.CheckBackLog();
        Destroy(gameObject);
    }
}