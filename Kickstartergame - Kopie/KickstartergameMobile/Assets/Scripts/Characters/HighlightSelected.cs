using UnityEngine;
using System.Collections;

public class HighlightSelected : MonoBehaviour {

	//// Use this for initialization
	//public static Material shaderMatOutline;
	//public static Material shaderMatSelected;
 //   public Material playerMaterial;
 //   private Material[] playerMaterials;
	//private Material[] normalMaterials;
	//private Material[]outlinedMaterials;
	//private Material[]selectedMaterials;
	//private bool init=true;
	//private bool hovered = false;
	//private bool selected = false;
	//public bool Hovered{
	//	get { return hovered; }
	//	set {
	//		hovered = value;
	//		SkinnedMeshRenderer m = GetComponentInChildren<SkinnedMeshRenderer> ();
 //           if(m== null)
 //                m = GetComponent<SkinnedMeshRenderer>();
 //           if (m.materials != null)
 //           {
 //               if (hovered && playerMaterials != null)
 //               {
 //                   m.materials = outlinedMaterials;//TODO outlineMaterial
 //               }
 //               else if (selected)
 //                   m.materials = selectedMaterials;
 //               else if (normalMaterials != null)
 //                   m.materials = normalMaterials;
 //           }
	//	}

	//}

	//void Start () {
	//	SkinnedMeshRenderer m = GetComponentInChildren<SkinnedMeshRenderer> ();
 //       if (m == null)
 //           m = GetComponent<SkinnedMeshRenderer>();
 //       normalMaterials = m.materials;
 //       outlinedMaterials = new Material[m.materials.Length];
 //       selectedMaterials = new Material[m.materials.Length];
 //       playerMaterials = new Material[m.materials.Length];
	//}
	//public void Selected(bool value){
	//	selected = value;
	//	SkinnedMeshRenderer m = GetComponentInChildren<SkinnedMeshRenderer> ();
 //       if (m == null)
 //           m = GetComponent<SkinnedMeshRenderer>();
 //       if (value)
	//		m.materials = selectedMaterials;
	//	else {
	//		m.materials = normalMaterials;
	//	}
	//}
	//void OnMouseEnter(){
        
	//	if (!GetComponent<CharacterScript>().getCharacter().Selected) {
 //           SkinnedMeshRenderer m = GetComponentInChildren<SkinnedMeshRenderer> ();
 //           if (m == null)
 //               m = GetComponent<SkinnedMeshRenderer>();
 //           m.materials = outlinedMaterials;
	//	}
	//}
	//void OnMouseExit(){
	//	if (!GetComponent<CharacterScript>().getCharacter().Selected) {
	//		SkinnedMeshRenderer m = GetComponentInChildren<SkinnedMeshRenderer> ();
 //           if (m == null)
 //               m = GetComponent<SkinnedMeshRenderer>();
	//			m.materials = normalMaterials;
	//	}
	//}
	//// Update is called once per frame
	//void Update () {
		
	//	if (init) {
	//		SkinnedMeshRenderer m = GetComponentInChildren<SkinnedMeshRenderer> ();
 //           if (m == null)
 //               m = GetComponent<SkinnedMeshRenderer>();
            
 //           for (int i = 0; i < m.materials.Length; i++)
 //           {
 //               if (shaderMatOutline != null)
 //               {//TODO
 //                   playerMaterials[i] = new Material(shaderMatOutline.shader);
 //                   playerMaterials[i].CopyPropertiesFromMaterial(playerMaterial);
 //                   playerMaterials[i].mainTexture = m.materials[i].mainTexture;

 //                   playerMaterials[i].color = m.materials[i].color;

 //                   selectedMaterials[i] = new Material(shaderMatSelected.shader);
 //                   selectedMaterials[i].CopyPropertiesFromMaterial(shaderMatSelected);
 //                   selectedMaterials[i].mainTexture = m.materials[i].mainTexture;

 //                   selectedMaterials[i].color = m.materials[i].color;

 //                   outlinedMaterials[i] = new Material(shaderMatSelected.shader);
 //                   outlinedMaterials[i].CopyPropertiesFromMaterial(shaderMatSelected);
 //                   outlinedMaterials[i].mainTexture = m.materials[i].mainTexture;

 //                   outlinedMaterials[i].color = m.materials[i].color;
 //               }
 //           }
	//		init = false;
	//	}

	//}
}
