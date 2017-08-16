using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Items;
using Assets.Scripts.GameStates;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterScript :  MonoBehaviour {
    [HideInInspector]
	public Character character;
    public static bool lockInput=false;
	private Boolean init = true;
    [HideInInspector]
    public Animator animator;
    public AudioClip voice;
    public AudioClip death;
    public AudioClip run;
    public GameObject animatedText=null;
    [HideInInspector]
    public Material[] startMaterials;
	const float TAUNT_TIME=1.0f;
    float dragtime = 0;
    const float DRAG_DELAY = 0.25f;
    [HideInInspector]
    public GameObject arrow;
    Vector3 jumpPosition;
    private bool taunt = false;
    private float taunttime = 0;
    public static bool drag = false;
    bool dragging = false;
    bool delayDrag = true;
    bool dragMaterial = false;
    Vector3 dist;
    float posX;
    float posY;
    /*
    // public ControllerServer server;
    // Use this for initialization
    public Character getCharacter()
    {
        return character;
    }
    public void setCharacter(Character c)
    {
        character = c;
    }
    public void SetColor(Color c)
    {
        int cnt = 0;
        SpriteRenderer[] s = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in s)
        {
            if (cnt < 2)
                sr.color = c;
            cnt++;
        }
    }
    public void SetHPBar()
    {
        HPBarOnMap hpBar = GetComponentInChildren<HPBarOnMap>();
        hpBar.SetActive(character);
    }
	void Start () {
        //character = new Character("name",CharacterClassType.SwordFighter,weaponPosition);
		//HPBarOnMap hpBar = GetComponentInChildren<HPBarOnMap> ();
		//hpBar.SetActive (character);
        animator = GetComponentInChildren<Animator>();
	}

    #region AnimationEvents
    public void SetAnimator()
    {
        animator = GetComponentInChildren<Animator>();
    }
    IEnumerator DelayAnimatedText(string text,Color c,float delay, int fontsize)
    {
        yield return new WaitForSeconds(delay);
        GameObject animatedText = GameObject.Instantiate(FindObjectOfType<GUIPrefabs>().animatedText, this.gameObject.GetComponentInChildren<Canvas>().transform);
        animatedText.transform.localPosition = new Vector3();
        animatedText.GetComponent<Text>().text = text;
        animatedText.GetComponent<Text>().color = c;
        Debug.Log(c);
        animatedText.GetComponent<Text>().fontSize = fontsize;
        //animatedText.transform.LookAt(Camera.main.transform);
        StartCoroutine(DisableGameObjectAfterDelay(animatedText, 2));
    }
    public void StartAnimatedText(string text, Color c, float delay, int fontsize)
    {
            StartCoroutine(DelayAnimatedText(text, c,delay,fontsize));
            
    }
    List<Color> oldMaterials;
    IEnumerator FadeWaitAnimation(float brightness)
    {
        float time = 0;
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            oldMaterials.Add(mr.material.color);
        }
        foreach (SkinnedMeshRenderer mr in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            oldMaterials.Add(mr.material.color);
        }
        float speed = 16;
        while (time <= 1)
        {
            yield return new WaitForSeconds(0.02f);
            time += 0.02f*speed;
            int i = 0;
            foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
            {
                mr.material.color = Color.Lerp(oldMaterials[i],new Color(brightness, brightness, brightness),time);
                i ++;
            }
            foreach (SkinnedMeshRenderer mr in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                mr.material.color = Color.Lerp(oldMaterials[i], new Color(brightness, brightness, brightness), time);
                i++;
            }

        }
    }
    public void WaitAnimation(bool wait)
    {
        if (wait)
        {
            oldMaterials = new List<Color>();
            float brightness = 0.3f;
            if (character.characterClassType == Assets.Scripts.Characters.Classes.CharacterClassType.SwordFighter)
                brightness = 0.5f;
            StartCoroutine(FadeWaitAnimation(brightness));

        }
        else
        {
            int i = 0;
            foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
            {
                mr.material.color = oldMaterials[i];
                i++;
            }
            foreach (SkinnedMeshRenderer mr in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                mr.material.color = oldMaterials[i];
                i++;
            }
        }
    }
    IEnumerator DisableGameObjectAfterDelay(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Destroy(go);
    }
    public void setRunning()
    {
        //Destroy(trail);
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Running", true);
    }
    public void SetSelected(bool selected)
    {
        animator.SetBool("Selected", selected);
        //Debug.Log("Selected");
        
    }
    public void setIdle()
    {
        animator.SetBool("Running", false);
    }

    public void setAttacking(){
		if (character.characterClassType == Assets.Scripts.Characters.Classes.CharacterClassType.Archer) {
			arrow = Instantiate(GameObject.Find("RessourceScript").GetComponent<WeaponScript>().arrowHolding,this.GetComponentInChildren<ArrowPosition>().transform.position,Quaternion.identity) as GameObject;
			arrow.transform.SetParent (this.GetComponentInChildren<ArrowPosition> ().transform, false);
			Debug.Log (GameObject.Find ("RessourceScript").GetComponent<WeaponScript> ().arrowHolding.transform.position);
			Debug.Log (GameObject.Find ("RessourceScript").GetComponent<WeaponScript> ().arrowHolding.transform.localPosition);
			Debug.Log (GameObject.Find ("RessourceScript").GetComponent<WeaponScript> ().arrowHolding.transform.rotation);
			Debug.Log (GameObject.Find ("RessourceScript").GetComponent<WeaponScript> ().arrowHolding.transform.localRotation);
			arrow.transform.localPosition = GameObject.Find("RessourceScript").GetComponent<WeaponScript>().arrowHolding.transform.position;
			arrow.transform.localRotation = GameObject.Find("RessourceScript").GetComponent<WeaponScript>().arrowHolding.transform.rotation;
		}
		animator.SetTrigger ("Attack1Trigger");
	}
    public void setIdleCreator()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetTrigger("IdleCreator");
    }
    public void setAttacking2()
    {
        animator.SetTrigger("Attack2Trigger");
    }
	public void SetInjuredIdle(bool value)
	{
		if (animator == null)
			animator=GetComponentInChildren<Animator> ();
		animator.SetBool ("InjuredIdle", value);
	}
    public void Jump(Vector3 position)
    {
        jumpPosition = position;
        animator.SetTrigger("JumpTrigger");
    }
    public void JumpAnimationEvent()
    {
    }
	public void GetHitAnimation()
	{
		animator.SetTrigger("Hit1Trigger");
	}
	public void GetHit2Animation()
	{
		animator.SetTrigger("Hit2Trigger");
	}
	public void PlayDogueAnimation()
	{
		animator.SetTrigger("Dodge");
	}
    public void HitGroundAnimation()
    {
        animator.SetTrigger("Attack2Trigger");
    }
    public void setChargeAttack()
    {
        animator.SetTrigger("ChargeAttackTrigger");
    }
	public void StandUpAnimation()
	{
		animator.SetTrigger("StandUp");
	}
	public void FallAnimation()
	{
		animator.SetTrigger("Falling");
	}
    public void FallStandUpAnimation()
    {
        animator.SetTrigger("FallingStandup");
    }
	public void StopFalling()
	{
		animator.SetTrigger("StopFalling");
	}
    public void Kneeling()
    {
        animator.SetTrigger("Kneeling");
    }
	public void StartGroundMovement()
	{
		animator.SetBool ("GroundMoving", true);
	}
	public void StopGroundMovement()
	{
		animator.SetBool ("GroundMoving", false);
	}
	public void PlaySpearSlamAnimation()
	{
		animator.SetTrigger("SpearSlam");
	}
	public void PlayIceBlockAnimation()
	{
		animator.SetTrigger("IceblockTrigger");
	}
	public void PlayAreaAttackAnimation()
	{
		animator.SetTrigger("AreaAttackTrigger");
	}
	public void PlayBattleCryAnimation()
	{
		animator.SetTrigger("BattleCry");
	}
    public void StartTalkingAnimation()
    {
        animator.SetTrigger("StartTalking");
    }
    public void EndTalkingAnimation()
    {
        animator.SetTrigger("EndTalking");
    }
    public void LookingAround()
    {
        animator.SetTrigger("LookingAround");
    } 
	public void PlayTauntAnimation()
	{
		taunt = true;
	}
    public void setHeal()
    {
        animator.SetTrigger("Heal");
    }
	public void ForceIdle(){
		animator.SetTrigger ("ForceIdle");
	}
    public void PlayVoice()
    {
        GetComponent<AudioSource>().clip = voice;
        GetComponent<AudioSource>().Play();
    }
    public void PlayRun()
    {
        GetComponent<AudioSource>().clip = run;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().volume = 0.14f;
       GetComponent<AudioSource>().Play();
    }
	public void PlayOpenDoorAnimation(){
		animator.SetTrigger("OpenDoor");
	}
	public void PlayOpenChestAnimation(){
		//animator.SetTrigger("OpenChest");
	}
    public void StopRun()
    {
        GetComponent<AudioSource>().loop = false;
        GetComponent<AudioSource>().volume = 0.1f;
        GetComponent<AudioSource>().Stop(); ;
    }
    public void PlayDeath()
    {
		if (character.team == 0) {
			Time.timeScale = 0.3f;
			CameraMovement.Follow (GetComponentInChildren<CameraFollow>().transform);
		}

        SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer s in sr)
        {
            s.enabled = false;
        }
        GetComponent<BoxCollider>().enabled = false;
        animator.SetTrigger("Death");
        GetComponent<AudioSource>().clip = death;
        GetComponent<AudioSource>().Play();
    }
    #endregion

	void Update () {
	
        if (character != null)
        {
			if (taunt) {
				taunttime += Time.deltaTime;
				if (taunttime > TAUNT_TIME) {
					taunttime = 0;
					taunt = false;
					animator.SetTrigger("Taunt");
				}
			}
        }
        if (dragging)
        {
            if (Input.GetMouseButton(0))
            {
                dragtime += Time.deltaTime;
                if(dragtime >= DRAG_DELAY)
                {
                    drag = true;
                    delayDrag = false;
                }
            }
            else
            {
                dragging = false;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (!delayDrag)
                {
                    
                    FindObjectOfType<UXRessources>().movementFlag.SetActive(false);
                    dragging = false;
                    drag = false;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit = new RaycastHit();
                    //Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain"));
                    Physics.Raycast(ray, out hit, Mathf.Infinity);
                    int x = (int)Mathf.Floor(hit.point.x);
                    int y = (int)Mathf.Floor(hit.point.y);
                    GameObject.Find("AttackIcon").GetComponent<Image>().enabled = false;
                    FindObjectOfType<DragCursor>().GetComponentInChildren<MeshRenderer>().enabled = false;
                    // ChangeToStartMaterial();
                    dragMaterial = false;
                    if (hit.collider.gameObject.tag == "Grid")
                    {
                        if (FindObjectOfType<GridScript>().fields[x, y].isActive && !(x == character.x && y == character.y))
                        {
                            character.SetPosition(character.x, character.y);
                            FindObjectOfType<MainScript>().MoveCharacterTo(character, MouseManager.oldMousePath, true, new GameplayState());

                        }
                        else if (FindObjectOfType<GridScript>().fields[x, y].character != null && FindObjectOfType<GridScript>().fields[x, y].character.team != character.team)
                        {
                            FindObjectOfType<MainScript>().GoToEnemy(character, FindObjectOfType<GridScript>().fields[x, y].character, true);
                        }
                        else
                        {
                            FindObjectOfType<MainScript>().DeselectActiveCharacter();
                        }
                    }
                    else if (hit.collider.gameObject.GetComponent<CharacterScript>() != null)
                    {
                        Character ch = hit.collider.gameObject.GetComponent<CharacterScript>().character;
                        if (ch.team != character.team)
                        {
                            if(FindObjectOfType<GridScript>().IsFieldAttackable(ch.x,ch.z))
                                FindObjectOfType<MainScript>().GoToEnemy(character, ch, true);
                        }
                        else
                        {
                            Debug.Log("Deselect2");
                            FindObjectOfType<MainScript>().DeselectActiveCharacter();
                        }
                    }
                    else
                    {
                        Debug.Log("Deselect1");
                        FindObjectOfType<MainScript>().DeselectActiveCharacter();
                    }
                }
            }
        }
        else
        {
            GetComponent<BoxCollider>().enabled = true;
            //  GameObject.Find("AttackIcon").GetComponent<Image>().enabled = false;
        }
	}
    public void LevelUp()
    {
        CameraMovement.moveToFinishedEvent += SpawnLevelUpOrbs;
    }
    IEnumerator DelayOrbSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject orbs = GameObject.Instantiate(GameObject.FindObjectOfType<EffectsScript>().LevelUpOrbs);
        orbs.GetComponent<LevelUpOrbs>().LevelUp(character);
        CameraMovement.moveToFinishedEvent -= SpawnLevelUpOrbs;
    }
    public void FadeOutLevelUpEffect()
    {
        StartCoroutine(FadeOutLevelEffect(0.5f));
        CharacterScript.LevelUpEffect.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
    float fadeTime = 0;
    IEnumerator FadeOutLevelEffect(float duration)
    {
        while (fadeTime <= 1)
        {
            foreach (MeshRenderer mr in CharacterScript.LevelUpEffect.GetComponentsInChildren<MeshRenderer>())
            {
                mr.material.SetColor("_TintColor", new Color(1, 1, 1, 1 - fadeTime));

            }
            fadeTime += 0.02f / duration;

            yield return new WaitForSeconds(0.02f);
        }
        foreach (MeshRenderer mr in CharacterScript.LevelUpEffect.GetComponentsInChildren<MeshRenderer>())
        {

            mr.material.SetColor("_TintColor", new Color(1, 1, 1, 0));
        }
        yield return new WaitForSeconds(0.2f);
        GameObject.Destroy(LevelUpEffect);
    }
    public static GameObject LevelUpEffect;
    public void SpawnLevelUpOrbs()
    {
        CameraMovement.locked = true;
        //BloodPosition pos = this.gameObject.GetComponentInChildren<BloodPosition>();
        LevelUpEffect=GameObject.Instantiate(GameObject.Find("RessourceScript").GetComponent<EffectsScript>().newLevelUpEffect, transform.position, Quaternion.identity)as GameObject;
        StartCoroutine(DelayOrbSpawn(1.25f));
        
    }

    void EndOfMove()
    {
        MainScript.endOfMoveCharacterEvent -= EndOfMove;
        //FindObjectOfType<MainScript>().ActiveCharWait();
    }

	void OnMouseEnter(){
		if (!EventSystem.current.IsPointerOverGameObject ()) {
            //GameObject.Find (MainScript.MAIN_GAME_OBJ).GetComponent<MainScript> ().ShowCharacterInfo (character);

            // if (drag)
            ActiveUnitEffect a =GetComponentInChildren<ActiveUnitEffect>();
            if (a != null)
            {
                a.SetHovered(true);
            }
            if(FindObjectOfType<MainScript>().activeCharacter!=null && FindObjectOfType<MainScript>().activeCharacter !=character)
            {
                MouseManager.DraggedOver(character);
            }
        }
    }
    void OnMouseOver()
    {
        
		//if(GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().lastClickedCharacter != character)
       // GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().UpdateCharacterInfo();
    }
    void OnMouseExit(){
        character.hovered = false;
        ActiveUnitEffect a = GetComponentInChildren<ActiveUnitEffect>();
        if (a != null)
        {
            a.SetHovered(false);
        }
        if (drag)
        {
            MouseManager.DraggedExit();
        }
    }

    void OnMouseDrag()
    {
        if (lockInput)
            return;
        if (!character.IsWaiting &&character.isAlive&& character.team == MainScript.ActivePlayerNumber)
        {
            dragging = true;
            
            if (!delayDrag)
            {
                GetComponent<BoxCollider>().enabled = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                // Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain"));
                Physics.Raycast(ray, out hit, Mathf.Infinity);
                if (hit.collider.tag == "Grid")
                {
                    int x = (int)Mathf.Floor(hit.point.x);
                    int z = (int)Mathf.Floor(hit.point.z);
                    if (MouseManager.active)
                        MouseManager.CharacterDrag(x, z, character);
                }
                else if (hit.collider.GetComponent<CharacterScript>() != null)
                {
                    int x = hit.collider.GetComponent<CharacterScript>().character.x;
                    int z = hit.collider.GetComponent<CharacterScript>().character.y;
                    if (MouseManager.active)
                        MouseManager.CharacterDrag(x, z, character);
                }
            }
        }
    }
   

    void OnMouseDown()
    {
        if (!character.isAlive)
            return;
        if (lockInput)
            return;
        delayDrag = true;
        dragtime = 0;
        dist = Camera.main.WorldToScreenPoint(transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;
        MouseManager.oldMousePath = new List<Vector2>(MouseManager.mousePath);
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            GameObject.Find("AttackIcon").GetComponent<Image>().enabled = false;
            MainScript.characterClickedEvent(character);
        }
    }
        
*/
}
