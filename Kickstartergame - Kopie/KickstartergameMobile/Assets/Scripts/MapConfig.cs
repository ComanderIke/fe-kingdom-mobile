using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapConfig : MonoBehaviour {

    public int gridWidth;
    public int gridHeight;
    private StartPosition[] startPositions;
    private EnemyPosition[] enemyPositions;
    private NeutralPosition[] neutralPositions;
    // Use this for initialization
    void Start () {
        LoadPositions();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public StartPosition[] GetStartPositions()
    {
        return startPositions;
    }
    public EnemyPosition[] GetEnemyPositions()
    {
        return enemyPositions;
    }
    public NeutralPosition[] GetNeutralPositions()
    {
        return neutralPositions;
    }
    void LoadPositions()
    {
        startPositions = GetComponentsInChildren<StartPosition>();
        enemyPositions = GetComponentsInChildren<EnemyPosition>();
        neutralPositions = GetComponentsInChildren<NeutralPosition>();
    }
}
