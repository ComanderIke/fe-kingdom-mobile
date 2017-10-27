using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Items;
using Assets.Scripts.GameStates;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.Characters;
using Assets.Scripts.Grid;

public class MovableObject :  MonoBehaviour {
    [HideInInspector]
	public LivingObject unit;
    public static bool lockInput=false;
	private Boolean init = true;
    [HideInInspector]
    public Animator animator;
    public GameObject animatedText=null;
    [HideInInspector]
    public Material[] startMaterials;
    float dragtime = 0;
    const float DRAG_DELAY = 0.15f;
    Vector3 jumpPosition;
    public static bool drag = false;
    bool dragging = false;
    bool delayDrag = true;
    bool dragMaterial = false;
    Vector3 dist;
    float posX;
    float posY;
    Vector2 posBeforeDrag;
    
    public LivingObject GetUnit()
    {
        return unit;
    }

	void Start () {
        animator = GetComponentInChildren<Animator>();
	}
    
    #region AnimationEvents
    /*
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
    */
    #endregion

	void Update () {
	
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
                    //FindObjectOfType<UXRessources>().movementFlag.SetActive(false);
                    dragging = false;
                    drag = false;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit = new RaycastHit();
                    //Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain"));
                    Physics.Raycast(ray, out hit, Mathf.Infinity);
                    int x = (int)Mathf.Floor(hit.point.x-GridScript.GRID_X_OFFSET);
                    int y = (int)Mathf.Floor(hit.point.y);
                    FindObjectOfType<DragCursor>().GetComponentInChildren<MeshRenderer>().enabled = false;
                    // ChangeToStartMaterial();
                    dragMaterial = false;
                    transform.position = posBeforeDrag;
                    Debug.Log(hit.collider.name);
                    if (hit.collider.gameObject.tag == "Grid")
                    {
                        if (FindObjectOfType<GridScript>().fields[x, y].isActive && !(x == unit.x && y == unit.y))
                        {
                            unit.SetPosition(unit.x, unit.y);
                            //int centerX = (int)Mathf.Round(hit.point.x - GridScript.GRID_X_OFFSET) - 1;
                            //int centerY = (int)Mathf.Round(hit.point.y) - 1;
                            //BigTile clickedBigTile = MouseManager.GetClickedBigTile(centerX, centerY, x, y);

                           // MouseManager.CalculateMousePathToPositon(unit, clickedBigTile);
                            FindObjectOfType<MainScript>().MoveCharacterTo(unit, x , y, MouseManager.oldMousePath, true, new GameplayState());

                        }
                        else if (FindObjectOfType<GridScript>().fields[x, y].character != null && FindObjectOfType<GridScript>().fields[x, y].character.team != unit.team)
                        {
                           FindObjectOfType<MainScript>().GoToEnemy(unit, FindObjectOfType<GridScript>().fields[x, y].character, true);
                        }
                        else
                        {
                            FindObjectOfType<MainScript>().DeselectActiveCharacter();
                        }
                    }
                    else if (hit.collider.gameObject.GetComponent<MovableObject>() != null)
                    {
                        LivingObject ch = hit.collider.gameObject.GetComponent<MovableObject>().unit;
                        if (ch.team != unit.team)
                        {
                            if(FindObjectOfType<GridScript>().IsFieldAttackable(ch.x,ch.y))
                                FindObjectOfType<MainScript>().GoToEnemy(unit, ch, true);
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
        }
	}
    public void LevelUp()
    {
      
    }

    void EndOfMove()
    {
        MainScript.endOfMoveCharacterEvent -= EndOfMove;
    }

	void OnMouseEnter(){
		if (!EventSystem.current.IsPointerOverGameObject ()) {
            ActiveUnitEffect a =GetComponentInChildren<ActiveUnitEffect>();
            if (a != null)
            {
                a.SetHovered(true);
            }
            if(FindObjectOfType<MainScript>().activeCharacter!=null && FindObjectOfType<MainScript>().activeCharacter !=unit)
            {
                MouseManager.DraggedOver(unit);
            }
        }
    }
    void OnMouseExit(){
        unit.hovered = false;
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
        if (!unit.isWaiting &&unit.isAlive&& unit.team == MainScript.ActivePlayerNumber)
        {
            dragging = true;
            
            if (!delayDrag)
            {
                GetComponent<BoxCollider>().enabled = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                Physics.Raycast(ray, out hit, Mathf.Infinity);
                Vector3 curPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y - posY, 0);
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
                worldPos.z = 0;
                worldPos.x -= GridScript.GRID_X_OFFSET;
                transform.localPosition = Vector3.Lerp(transform.localPosition,worldPos,Time.deltaTime*13);
 
                int x = (int)Mathf.Floor(hit.point.x-GridScript.GRID_X_OFFSET);
                int y = (int)Mathf.Floor(hit.point.y);
                if (MouseManager.active)
                    MouseManager.CharacterDrag(x, y, unit);
            }
        }
    }


    void OnMouseDown()
    {

        if (!unit.isAlive)
            return;
        if (lockInput)
            return;
        delayDrag = true;
        dragtime = 0;
        dist = Camera.main.WorldToScreenPoint(transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;
        posBeforeDrag = transform.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        //Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain"));
        Physics.Raycast(ray, out hit, Mathf.Infinity);
        int x = (int)Mathf.Floor(hit.point.x-GridScript.GRID_X_OFFSET);
        int y = (int)Mathf.Floor(hit.point.y);
        MouseManager.currentX = x;
        MouseManager.currentY = y;
        MouseManager.oldMousePath = new List<Vector2>(MouseManager.mousePath);
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            MainScript.characterClickedEvent(unit);
        }
    }
        

}
