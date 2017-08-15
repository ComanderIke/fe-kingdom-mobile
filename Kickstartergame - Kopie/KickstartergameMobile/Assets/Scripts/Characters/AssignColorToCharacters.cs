using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Characters;

public class AssignColorToCharacters : MonoBehaviour {

	private Color32 color=Color.green;
	public Material materialWithOutlineShaderAttributes;
	public Material materialWithSelectedOutlineShaderAttributes;
    public Material PlayerMaterial;
    const int WARRIOR_TEAMMATERIAL_INDEX = 4;
    const int NINJA_TEAMMATERIAL_INDEX = 6;
    const int MAGE_TEAMMATERIAL_INDEX = 5;
    const int MAGE_TEAMMATERIAL_INDEX2 = 0;

    public Shader outlineShader;
	public Shader selectedOutlineShader;
	private bool init=true;
    // Use this for initialization
    public void ApplyColor()
    {
        /*SkinnedMeshRenderer[] m = GetComponentsInChildren<SkinnedMeshRenderer>();
        
        for (int a = 0; a < m.Length; a++)
        {
           List<int> teamColormaterialindex = new List<int>();
            if(m[a].gameObject.transform.parent.transform.parent.GetComponent<CharacterScript>().getCharacter().charclass.weaponType.type == WeaponType.Sword)
            {
                teamColormaterialindex.Add(WARRIOR_TEAMMATERIAL_INDEX);
            }
            else if (m[a].gameObject.transform.parent.transform.parent.GetComponent<CharacterScript>().getCharacter().charclass.weaponType.type == WeaponType.Spear)
            {
                teamColormaterialindex.Add(WARRIOR_TEAMMATERIAL_INDEX);
            }
            else if (m[a].gameObject.transform.parent.transform.parent.GetComponent<CharacterScript>().getCharacter().charclass.weaponType.type == WeaponType.Dagger)
            {
                teamColormaterialindex.Add(NINJA_TEAMMATERIAL_INDEX);
            }
            else if (m[a].gameObject.transform.parent.transform.parent.GetComponent<CharacterScript>().getCharacter().charclass.weaponType.type == WeaponType.Bow)
            {
                teamColormaterialindex.Add(NINJA_TEAMMATERIAL_INDEX);
            }
            else if (m[a].gameObject.transform.parent.transform.parent.GetComponent<CharacterScript>().getCharacter().charclass.weaponType.type == WeaponType.Magic)
            {
				teamColormaterialindex.Add(WARRIOR_TEAMMATERIAL_INDEX);
            }
            else if (m[a].gameObject.transform.parent.transform.parent.GetComponent<CharacterScript>().getCharacter().charclass.weaponType.type == WeaponType.Staff)
            {
                teamColormaterialindex.Add(MAGE_TEAMMATERIAL_INDEX);
                teamColormaterialindex.Add(MAGE_TEAMMATERIAL_INDEX2);
            }
            for (int i = 0; i < m[a].materials.Length; i++)
            {
                if (teamColormaterialindex.Contains(i))
                {
                    m[a].materials[i].color = color;
                }
               // m[a].materials[7].color = color;
            }
        }
        */
        CharacterScript[] cs = GetComponentsInChildren<CharacterScript>();
        foreach(CharacterScript c in cs)
        {
            c.SetColor(color);
        }
       

    }
    void Awake()
    {
        //HighlightSelected.shaderMatOutline = materialWithOutlineShaderAttributes;
        //HighlightSelected.shaderMatSelected = materialWithSelectedOutlineShaderAttributes;
        //HighlightSelected[] hs = GetComponentsInChildren<HighlightSelected>();
        //foreach(HighlightSelected h in hs)
        //{
        //    h.playerMaterial = PlayerMaterial;
        //}
    }
	void Start () {
		/*
		SkinnedMeshRenderer []m = GetComponentsInChildren<SkinnedMeshRenderer> ();
        SpriteRenderer[] s=GetComponentsInChildren<SpriteRenderer>();

        for (int a = 0; a < m.Length; a++)
        {
            List<int> teamColormaterialindex = new List<int>();
            if (m[a].gameObject.transform.parent.transform.parent.GetComponent<CharacterScript>().getCharacter().charclass.weaponType.type == WeaponType.Sword)
            {
                teamColormaterialindex.Add(WARRIOR_TEAMMATERIAL_INDEX);
            }
            else if (m[a].gameObject.transform.parent.transform.parent.GetComponent<CharacterScript>().getCharacter().charclass.weaponType.type == WeaponType.Dagger)
            {
                teamColormaterialindex.Add(NINJA_TEAMMATERIAL_INDEX);
            }
            else if (m[a].gameObject.transform.parent.transform.parent.GetComponent<CharacterScript>().getCharacter().charclass.weaponType.type == WeaponType.Magic)
            {
                teamColormaterialindex.Add(MAGE_TEAMMATERIAL_INDEX);
                teamColormaterialindex.Add(MAGE_TEAMMATERIAL_INDEX2);
            }
            for (int i = 0; i < m[a].materials.Length; i++)
            {
                if (teamColormaterialindex.Contains(i))
                {
                    m[a].materials[i].color = color;
                }
                // m[a].materials[7].color = color;
            }
        }
        */
        CharacterScript[] cs = GetComponentsInChildren<CharacterScript>();
        foreach (CharacterScript c in cs)
        {
            c.SetColor(color);
        }


    }
	public void setColor(Color c){
		color = c;
	}
	
	// Update is called once per frame
	void Update () {
		
    }
}
