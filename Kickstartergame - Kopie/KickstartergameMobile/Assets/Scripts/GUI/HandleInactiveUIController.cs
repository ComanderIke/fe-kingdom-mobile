using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HandleInactiveUIController : MonoBehaviour {

	#region input
	public Material activeMaterial;
	public Material inactiveMaterial;
	public GameObject[] allUIObjects;
	public Text[] allTextWhiteObjects;
	public Text[] allYellowTextObjects;
	public Text[] allOrangeTextObjects;
	public Text[] allBlueTextObjects;
	public Text[] allAttributeTextObjects;
	public Text[] allSkillTextObjects;
	public Color grey = new Color (0.1f, 0.1f, 0.1f, 1);
	public Color orange;
	public Color blue;
	#endregion

	static public Material m_activeMaterial;
	static public Material m_inactiveMaterial;
	static public GameObject[] m_allUIObjects;
	static public Text[] m_allTextWhiteObjects;
	static public Text[] m_allYellowTextObjects;
	static public Text[] m_allOrangeTextObjects;
	static public Text[] m_allBlueTextObjects;
	static public Text[] m_allAttributeTextObjects;
	static public Text[] m_allSkillTextObjects;
	static public Color m_grey;
	static public Color m_orange;
	static public Color m_blue;

	// Use this for initialization
	void Start () {

		m_activeMaterial = activeMaterial;
		m_inactiveMaterial = inactiveMaterial;

		m_allUIObjects = allUIObjects;
		m_allTextWhiteObjects = allTextWhiteObjects;
		m_allYellowTextObjects = allYellowTextObjects;
		m_allOrangeTextObjects = allOrangeTextObjects;
		m_allBlueTextObjects = allBlueTextObjects;
		m_allAttributeTextObjects = allAttributeTextObjects;
		m_allSkillTextObjects = allSkillTextObjects;
		m_grey = grey;
		m_orange = allOrangeTextObjects [0].color;
		m_blue = allBlueTextObjects [0].color;
	}

	static public void ActivateAll() {
		for (int i = 0; i < m_allUIObjects.Length; i++) {			
			if (m_allUIObjects [i] != null) {
				SetMaterial(m_allUIObjects[i], m_activeMaterial);		
			}
		}
		ActivateAllTextObjects ();
	}

	static public void DeactivateAll() {
		for (int i = 0; i < m_allUIObjects.Length; i++) {			
			if (m_allUIObjects [i] != null) {
				SetMaterial(m_allUIObjects[i], m_inactiveMaterial);		
			}
		}
		DeactivateAllTextObjects ();
	}

	static public void ActivateOnlyHealing() {
		for (int i = 0; i < m_allUIObjects.Length; i++) {			
			if (m_allUIObjects [i].name != "InventoryButton1 (1)" && m_allUIObjects [i] != null) {
				SetMaterial (m_allUIObjects [i], m_inactiveMaterial);	
			} 
		}
		DeactivateAllTextObjects ();
	}

	static public void ActivateOnlyAttributes() {
		for (int i = 0; i < m_allUIObjects.Length; i++) {			
			if (m_allUIObjects[i].name != "AttributeButton1" &&
				m_allUIObjects[i].name != "AttributeButton2" &&
				m_allUIObjects[i].name != "AttributeButton3" &&
				m_allUIObjects[i].name != "AttributeButton4" &&
				m_allUIObjects[i].name != "AttributeButton5" &&
				m_allUIObjects[i].name != "backGroundImageOnlyAttributes" &&

				m_allUIObjects [i] != null) {
				SetMaterial(m_allUIObjects[i], m_inactiveMaterial);	
			}
		}
		DeactivateAllTextObjects ();
		for (int i = 0; i < m_allAttributeTextObjects.Length; i++) {
			m_allAttributeTextObjects [i].color = Color.white;
		}
	}

	static public void ActivateOnlySkills () {
		for (int i = 0; i < m_allUIObjects.Length; i++) {			
			if (m_allUIObjects[i].name != "SkillButton1" &&
				m_allUIObjects[i].name != "SkillButton2" &&
				m_allUIObjects[i].name != "SkillButton3" &&
				m_allUIObjects[i].name != "backGroundImageOnlySkills" &&


				m_allUIObjects [i] != null) {
				SetMaterial(m_allUIObjects[i], m_inactiveMaterial);	
			}
		}
		DeactivateAllTextObjects ();
		for (int i = 0; i < m_allSkillTextObjects.Length; i++) {
			m_allSkillTextObjects [i].color = Color.white;
		}
	}


	static private void SetMaterial (GameObject go, Material material) {
		Image currentImage = go.GetComponent<Image> ();
		if (currentImage != null) {
			currentImage.material = material;
		}	
	}		

	static private void DeactivateAllTextObjects() {
		foreach (Text t in m_allTextWhiteObjects) {
			t.color = m_grey;
		}
		foreach (Text t in m_allYellowTextObjects) {
			t.color = m_grey;
		}
		foreach (Text t in m_allOrangeTextObjects) {
			t.color = m_grey;
		}
		foreach (Text t in m_allBlueTextObjects) {
			t.color = m_grey;
		}
	}

	static private void ActivateAllTextObjects() {
		foreach (Text t in m_allTextWhiteObjects) {
			t.color = Color.white;
		}
		foreach (Text t in m_allYellowTextObjects) {
			t.color = Color.yellow;
		}
		foreach (Text t in m_allOrangeTextObjects) {
			t.color = m_orange;
		}
		foreach (Text t in m_allBlueTextObjects) {
			t.color = m_blue;
		}	
	}
}
