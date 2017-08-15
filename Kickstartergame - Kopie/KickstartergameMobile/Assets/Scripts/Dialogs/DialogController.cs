using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogController : MonoBehaviour
{
    public GameObject[] orderAvatar = new GameObject[2]; 
    public bool isDialogTextEnabled;
    public GameObject dialogSkipIcon;

    public const string AVATAR_LEILA = "AvatarLeila";
    public const string AVATAR_HECTOR = "AvatarHector";
    public const string AVATAR_FLORA = "AvatarFlora";
    public const string AVATAR_ELDRIC = "AvatarEldric";
    public const string AVATAR_UNKNOWN = "AvatarUnknown";

    private Vector3 position1;
    private Vector3 position2;
    private Vector3 scale1;
    private Vector3 scale2;

    private Image avatar;
    private GameObject dialogText;
    private Image dialogTextBackground;
    private DialogController instance;
    private GameObject currentAvatar;
    private Text subtitleInfoText;
    
    private float timer;
    public bool isSkipping;
    private bool justPressed;

    void Start()
    {
        instance = this;
        isDialogTextEnabled = true;
        dialogText = GameObject.Find("DialogText");
        dialogText.SetActive(false);
        subtitleInfoText = GameObject.Find("SubTitleInfo").GetComponent<Text>();

       // dialogTextBackground = GameObject.Find("DialogBackground").GetComponent<Image>();
        position1 = GameObject.Find("AvatarPos1").GetComponent<RectTransform>().position;
        position2 = GameObject.Find("AvatarPos2").GetComponent<RectTransform>().position;
        dialogSkipIcon = GameObject.Find("DialogSkipIcon");
        dialogSkipIcon.SetActive(false);
        scale1 = GameObject.Find("AvatarPos1").GetComponent<RectTransform>().localScale;
        scale2 = GameObject.Find("AvatarPos2").GetComponent<RectTransform>().localScale;
        timer = 0;
    }

    void Update()
    {
        CheckKeyPressed(KeyCode.U);
        DisableAvatarsAfter(30f);
    }
    Coroutine c;
    public void ShowVisualDialog(string avatarName, string text)
    {
        if (c != null) { 
            StopCoroutine(c);
        }
        if (!isSkipping)
        {
            
            currentAvatar = GameObject.Find(avatarName);
            ReOrderAvatar(currentAvatar);
            ActivatePrimaryAvatar(currentAvatar);

            if (isDialogTextEnabled)
            {
                c=StartCoroutine(AnimateText(text));
                //dialogTextBackground.enabled = true;
                dialogText.SetActive(true);
            }
        }
    }
    IEnumerator AnimateText(string s)
    {
        
        Text t = dialogText.GetComponentInChildren<Text>();
        for (int i = 0; i <(s.Length + 1); i+=2){
            t.text = s.Substring(0, i);
            yield return new WaitForSeconds(.0001f);
        }
        dialogSkipIcon.SetActive(true);
    }
    //public void ShowVisualDialog(string avatarName, float durationPart1, string textPart1, string textPart2)
    //{
    //    if (!isSkipping)
    //    {
    //        currentAvatar = GameObject.Find(avatarName);
    //        ReOrderAvatar(currentAvatar);
    //        ActivatePrimaryAvatar(currentAvatar);

    //        if (isDialogTextEnabled)
    //        {
    //            dialogText.text = textPart1;
    //            dialogText.enabled = true;
    //            //dialogTextBackground.enabled = true;
    //            instance.StartCoroutine(ShowTextPart2(durationPart1, textPart2));
    //        }
    //    }   
    //}

    public void HideVisualDialog()
    {
        DeactivatePrimaryAvatar();
        
        dialogText.SetActive(false);
        //dialogTextBackground.enabled = false;
        //dialogText.gettext = "";
        
    }

    

    private void ReOrderAvatar(GameObject currentAvatar)
    {
        if (orderAvatar[0] == null)
        {
            orderAvatar[0] = currentAvatar;
            orderAvatar[1] = null;

            RepositionAvatar(orderAvatar[0], position1);
        }
        else 
        {
            orderAvatar[1] = orderAvatar[0];
            orderAvatar[0] = currentAvatar;

            RepositionAvatar(orderAvatar[1], position2);
            RepositionAvatar(orderAvatar[0], position1);
        }
    }

    private void RepositionAvatar(GameObject go_primary, Vector3 position)
    {
        go_primary.GetComponent<RectTransform>().position = position;
    }

    private void ActivatePrimaryAvatar(GameObject primaryAvatar)
    {
        Image[] activePassiveAvatarImages = primaryAvatar.GetComponentsInChildren<Image>();
        timer = 0;

        activePassiveAvatarImages[0].enabled = true;
        activePassiveAvatarImages[1].enabled = false;
        activePassiveAvatarImages[2].enabled = true;

        //ChangeScale(activePassiveAvatarImages, scale1);
    }
    private void DeactivatePrimaryAvatar()
    {
		if (currentAvatar == null)
			return;
        Image[] activePassiveAvatarImages = currentAvatar.GetComponentsInChildren<Image>();

        activePassiveAvatarImages[0].enabled = true;
        activePassiveAvatarImages[1].enabled = true;
        activePassiveAvatarImages[2].enabled = false;

        //ChangeScale(activePassiveAvatarImages, scale2);
    }

    private void ChangeScale(Image[] activePassiveAvatarImages, Vector3 scale)
    {
        for (int i = 0; i < activePassiveAvatarImages.Length; i++)
        {
            activePassiveAvatarImages[0].gameObject.GetComponent<RectTransform>().localScale = scale;
        }
    }

    public void DeactivateAvatar(string avatarname)
    {
        Image[] deactivateAvatar = GameObject.Find(avatarname).GetComponentsInChildren<Image>();

        foreach (Image image in deactivateAvatar)
        {
            image.enabled = false;
        }
    }

    private void DisableAvatarsAfter(float time)
    {
        if (currentAvatar != null)
        {
            timer += Time.deltaTime;

            if (timer > time)
            {
                currentAvatar = null;
                DeactivateAvatar(AVATAR_LEILA);
                DeactivateAvatar(AVATAR_HECTOR);
                DeactivateAvatar(AVATAR_FLORA);
                DeactivateAvatar(AVATAR_ELDRIC);
                DeactivateAvatar(AVATAR_UNKNOWN);
            }
        }
    }

    private void CheckKeyPressed(KeyCode kc)
    {
        if (Input.GetKeyDown(kc) && !isDialogTextEnabled)
        {
            isDialogTextEnabled = true;
            justPressed = true;
            Debug.Log("pressed enabled");
        }
        if (!justPressed && Input.GetKeyDown(kc) && isDialogTextEnabled)
        {
            isDialogTextEnabled = false;
            Debug.Log("pressed disabled");
        }

        justPressed = false;

        if (isDialogTextEnabled)
        {
//            subtitleInfoText.enabled = true;
        }

        if (!isDialogTextEnabled)
        {
 //           subtitleInfoText.enabled = false;
        }
    }
}


