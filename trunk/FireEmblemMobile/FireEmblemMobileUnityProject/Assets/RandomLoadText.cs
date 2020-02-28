using Assets.Scripts.Ressources;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]
public class RandomLoadText : MonoBehaviour {

    public TextData texts;
    private TextMeshProUGUI textMesh;
    private string currentText;
	// Use this for initialization
	void OnEnable () {
        textMesh = GetComponent<TextMeshProUGUI>();
        ChangeText();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) )
        {
            ChangeText();
        }
	}
    public void ChangeText()
    {
        string text;
        while((text = texts.textData[Random.Range(0, texts.textData.Length)])==currentText) { }
        currentText = text;
        textMesh.text = currentText;
    }
}
