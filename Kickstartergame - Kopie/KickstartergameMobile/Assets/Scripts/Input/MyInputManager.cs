using UnityEngine;
using System.Collections;
using System;

public class MyInputManager : MonoBehaviour {

    public const bool GAMEPAD_INPUT = false;
    const float GAMEPAD_SPEED = 25;
    static float time = 0.0f;
	 float SLOWDOWNTIME = 0.4f;
    public float actuationPoint = 0.4f;
	static bool LeftButtonDown=false;
	static bool RightButtonDown=false;
	static bool UpButtonDown=false;
	static bool DownButtonDown=false;
    static int Count = 0;
    public static bool isLevelUpState = false;
    public static Character levelUpCharacter;

    private static bool isAButtonPressed = false;
    private static bool isAButtonDown = false;
    private static bool isBButtonDown = false;
    private static bool isBButton2sPressed = false;
    private static bool isYButtonDown = false;
    private static bool isXButtonDown = false;
    private static int playerCount = 0;
    private static int playerNumber = 0;
    private static float f;
    private static float f2;
    private static float oldf;
    private static float oldf2;
    private static int cnt = 0;
    private static float speed = 30.0f;
    public static GameObject position;
    public static bool fixedPosition = false;
    static float bbuttontime = 0;
    static bool firstLoad = true;
    //static Camera c;
    // Use this for initialization
    void Start () {
        loadedCharacters = new Character[2];
        
       
    }
    //void OnLevelWasLoaded()
    //{
    //    position = GameObject.Find("InputPosition");
    //   // c = GameObject.Find("Main Camera").GetComponent<Camera>();
    //}
    private static float angle;
    void Update()
    {
        //Debug.Log(angley);
        if (time < SLOWDOWNTIME)
        {
            time += Time.deltaTime;
        }
        playerNumber = MainScript.ActivePlayerNumber;
        playerNumber = 0;
        if (isLevelUpState)
        {
            playerNumber = levelUpCharacter.team-1;
            playerNumber = 0;
            fixedPosition = true;
        }

        #region Axis
        if (GAMEPAD_INPUT)
        {
            if (playerNumber == 0)
            {
                f = Input.GetAxis("P1_Horizontal");
                f2 = Input.GetAxis("P1_Vertikal");

            }
            else
            {
                f = Input.GetAxis("P2_Horizontal");
                f2 = Input.GetAxis("P2_Vertikal");
            }
        }
        #endregion

        #region angle
        angle = (float)(57 * Math.Atan2(f2, f));
        angle = 360 - angle;
        angle -= 90;
        if(angle > 360)
            angle -= 360;
        angle = 360 - angle;
        //Debug.Log(angle);
       // Debug.Log(f + " " + f2);
        if (f == 0 && f2 == 0)
            angle = -1;
        #endregion

        #region Speed
        if (Math.Abs(f) > 0.28 || Math.Abs(f2) > 0.28)
        {
            cnt++;

        }
        else
        {
            if (GAMEPAD_INPUT)
                speed = GAMEPAD_SPEED-10f;
            cnt = 0;
        }
        if (cnt > 80)
        {
            if (GAMEPAD_INPUT)
                speed = GAMEPAD_SPEED;
        }
        if (time > SLOWDOWNTIME)
        {
            time = 0.0f;
           
        }
        if (!fixedPosition)
        {
            if (position != null)
            {
                
                x += f * speed * Time.deltaTime;
                z += f2 * speed * Time.deltaTime;
                //Debug.Log(" "+x+" "+z);
                position.transform.localPosition = new Vector3(x, 0, -z);
            }
        }
        #endregion

        #region not used?!?!
        if (time > SLOWDOWNTIME && f > 0.1f)
        {
            time = 0.0f;
            RightButtonDown = true;
        }
        else
            RightButtonDown = false;
        if (time > SLOWDOWNTIME && f < -0.1f)
        {
            time = 0.0f;

            LeftButtonDown = true;
        }
        else
            LeftButtonDown = false;

        if ((time > SLOWDOWNTIME) && f2 > 0.1f)
        {
            time = 0.0f;

            DownButtonDown = true;
        }
        else
            DownButtonDown = false;

        if ((time > SLOWDOWNTIME) && f2 < -0.1f)
        {
            time = 0.0f;
            UpButtonDown = true;
        }
        else
            UpButtonDown = false;
        #endregion
       
        #region GAMEPAD
        if (GAMEPAD_INPUT)
        {
            if (playerNumber == 0)
            {
                if (Input.GetButtonDown("P1_Action"))
                {
                    MyInputManager.isAButtonDown = true;
                }
                else
                    MyInputManager.isAButtonDown = false;
                if (Input.GetButton("P1_Action"))
                {
                    MyInputManager.isAButtonPressed = true;
                }
                else
                    MyInputManager.isAButtonPressed = false;
                if (Input.GetButtonDown("P1_Back"))
                {
                    MyInputManager.isBButtonDown = true;
                }
                else
                    MyInputManager.isBButtonDown = false;
                if (Input.GetButtonDown("P1_X"))
                {
                    MyInputManager.isXButtonDown = true;
                }
                else
                    MyInputManager.isXButtonDown = false;
                if (Input.GetButtonDown("P1_Y"))
                {
                    MyInputManager.isYButtonDown = true;
                }
                else
                    MyInputManager.isYButtonDown = false;
                if (Input.GetButton("P1_Back"))
                    bbuttontime += Time.deltaTime;
                else
                {
                    bbuttontime = 0;
                    isBButton2sPressed = false;
                }
                if (bbuttontime >= 1.5f)
                {
                    isBButton2sPressed = true;
                }
            }
            else
            {
                if (Input.GetButtonDown("P2_Action"))
                {
                    MyInputManager.isAButtonDown = true;
                }
                else
                    MyInputManager.isAButtonDown = false;
                if (Input.GetButtonDown("P2_Back"))
                {
                    MyInputManager.isBButtonDown = true;
                }
                else
                    MyInputManager.isBButtonDown = false;
                if (Input.GetButtonDown("P2_X"))
                {
                    MyInputManager.isXButtonDown = true;
                }
                else
                    MyInputManager.isXButtonDown = false;
                if (Input.GetButtonDown("P2_Y"))
                {
                    MyInputManager.isYButtonDown = true;
                }
                else
                    MyInputManager.isYButtonDown = false;

            }
            
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                MyInputManager.isAButtonDown = true;
            }
            else
            {
                MyInputManager.isAButtonDown = false;
            }
            if (Input.GetMouseButtonDown(1))
            {
                MyInputManager.isBButtonDown = true;
            }
            else
            {
                MyInputManager.isBButtonDown = false;
            }
        }
        #endregion
    }
    static float x = 0;
    static float z = 0;
    static bool setCharacter = false;
    static bool loadCharacter = false;
    static int playerNumberSetCharacter = 0;
    static int playerNumberLoadCharacter = 0;
    static Character characterToSet;
    static Character[] loadedCharacters;
    public static void SetCharacter(int playernumber, Character character)
    {
        setCharacter = true;
        playerNumberSetCharacter = playerNumber;
        characterToSet = character;
    }
    public static Character GetLoadedCharacter(int index)
    {
        return loadedCharacters[index];
    }
    public static int LoadedCharacterNumber()
    {
        cnt = 0;
        for(int i=0; i < loadedCharacters.Length; i++)
        {
            if (loadedCharacters[i] != null)
                cnt++;
        }
        return cnt;
    }
    public static void LoadCharacter()
    {
        firstLoad = true;
        loadCharacter = true;
        //playerNumberLoadCharacter = playernumber;

    }
    public static bool IsAButtonPressed()
    {

        return isAButtonPressed;
    }
    public static bool IsAButtonDown(){
       
        return isAButtonDown;
	}
	public static bool IsBButtonDown(){
        return isBButtonDown;
	}
    public static int PlayerCount()
    {
        return playerCount;
    }
    public static bool IsBButton2sPressed()
    {
        bool tmp = isBButton2sPressed;
        if (isBButton2sPressed)
        {
            bbuttontime = 0;
            isBButton2sPressed = false;
        }
        
        return tmp;
    }
    public static bool IsXButtonDown()
    {
        return isXButtonDown;
    }
    public static bool IsYButtonDown()
    {
        return isYButtonDown;
    }
    public static bool IsLeftButtonDown(){
        if (LeftButtonDown) Count++;
		return LeftButtonDown;
	}
	public static bool IsRightButtonDown(){
        if (RightButtonDown) Count++;
		return RightButtonDown;
	}
	public static bool IsUpButtonDown(){
        if (UpButtonDown) Count++;
		return UpButtonDown;
	}
	public static bool IsDownButtonDown(){
        if (DownButtonDown) Count++;
		return DownButtonDown;
	}
    public static float GetAngle()
    {
        return angle;
    }
    public static void ResetLoading()
    {
        firstLoad = true;
        loadCharacter = false;
    }

    internal static float GetXAxis()
    {
        return f;
    }

    internal static float GetYAxis()
    {
        return f2;
    }
}
