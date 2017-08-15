using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class infoBoxes : MonoBehaviour {

	const float INFO_TIME=5.0f;
	bool show = false;
	float timer=0;
	List<Image> currentInfoBoxes;

	public GameObject initiateAttackInfoBox;
	public GameObject endTurnInfoBox;
	public GameObject takeInfoBox;
	public GameObject attackButtonInfoBox;
	public GameObject undoInfoBox;
	public GameObject waitInfoBox;
	public GameObject attackInfoBox;
	public GameObject openInfoBox;
	public GameObject changeWeaponInfoBox;
	public GameObject activeCharInfoBox;
	public GameObject gameGoalInfoBox;
	// Use this for initialization
	void Start () {
		currentInfoBoxes = new List<Image> ();
	}
	
	void Update(){
		if (!show)
			return;
		timer += Time.deltaTime;
		if (timer > INFO_TIME) {
			foreach (Image img in currentInfoBoxes) {
				img.enabled = false;
				Text[] texts = img.GetComponentsInChildren<Text> ();
				foreach (Text t in texts) {
					t.enabled = false;
				}
			}
			show = false;
		}
	}
	void InitiateAttackInfoBox(){
		timer = 0;
		show = true;
		Image img = initiateAttackInfoBox.GetComponent<Image> ();
		img.enabled = true;
		Text[] texts = img.GetComponentsInChildren<Text> ();
		foreach (Text t in texts) {
			t.enabled = true;
		}
		currentInfoBoxes.Add(img);
	}
	void EndTurnInfoBox(){
		timer = 0;
		show = true;
		Image img = endTurnInfoBox.GetComponent<Image> ();
		img.enabled = true;
		Text[] texts = img.GetComponentsInChildren<Text> ();
		foreach (Text t in texts) {
			t.enabled = true;
		}
		currentInfoBoxes.Add(img);

	}
	void TakeInfoBox(){
		timer = 0;
		show = true;
		Image img = takeInfoBox.GetComponent<Image> ();
		img.enabled = true;
		Text[] texts = img.GetComponentsInChildren<Text> ();
		foreach (Text t in texts) {
			t.enabled = true;
		}
		currentInfoBoxes.Add(img);

	}
	void UndoInfoBox(){

		timer = 0;
		show = true;
		Image img = undoInfoBox.GetComponent<Image> ();
		img.enabled = true;
		Text[] texts = img.GetComponentsInChildren<Text> ();
		foreach (Text t in texts) {
			t.enabled = true;
		}
		currentInfoBoxes.Add(img);
	}
	void WaitInfoBox(){

		timer = 0;
		show = true;
		Image img = waitInfoBox.GetComponent<Image> ();
		img.enabled = true;
		Text[] texts = img.GetComponentsInChildren<Text> ();
		foreach (Text t in texts) {
			t.enabled = true;
		}
		currentInfoBoxes.Add(img);
	}
	void AttackButtonInfoBox(){
		timer = 0;
		show = true;
		Image img = attackButtonInfoBox.GetComponent<Image> ();
		img.enabled = true;
		Text[] texts = img.GetComponentsInChildren<Text> ();
		foreach (Text t in texts) {
			t.enabled = true;
		}
		currentInfoBoxes.Add(img);

	}
	void AttackInfoBox(){

		timer = 0;
		show = true;
		Image img = attackInfoBox.GetComponent<Image> ();
		img.enabled = true;
		Text[] texts = img.GetComponentsInChildren<Text> ();
		foreach (Text t in texts) {
			t.enabled = true;
		}
		currentInfoBoxes.Add(img);
	}
	void ActiveCharInfoBox(){
		timer = 0;
		show = true;
		Image img = activeCharInfoBox.GetComponent<Image> ();
		img.enabled = true;
		Text[] texts = img.GetComponentsInChildren<Text> ();
		foreach (Text t in texts) {
			t.enabled = true;
		}
		currentInfoBoxes.Add(img);

	}
	void OpenInfoBox(){
		timer = 0;
		show = true;
		Image img = openInfoBox.GetComponent<Image> ();
		img.enabled = true;
		Text[] texts = img.GetComponentsInChildren<Text> ();
		foreach (Text t in texts) {
			t.enabled = true;
		}
		currentInfoBoxes.Add(img);

	}
	void GameGoalInfoBox(){
		timer = 0;
		show = true;
		Image img = gameGoalInfoBox.GetComponent<Image> ();
		img.enabled = true;
		Text[] texts = img.GetComponentsInChildren<Text> ();
		foreach (Text t in texts) {
			t.enabled = true;
		}
		currentInfoBoxes.Add(img);

	}
	void ChangeWeaponInfoBox(){
		timer = 0;
		show = true;
		Image img = changeWeaponInfoBox.GetComponent<Image> ();
		img.enabled = true;
		Text[] texts = img.GetComponentsInChildren<Text> ();
		foreach (Text t in texts) {
			t.enabled = true;
		}
		currentInfoBoxes.Add(img);

	}
}
