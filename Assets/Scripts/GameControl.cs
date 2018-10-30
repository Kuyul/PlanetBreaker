using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    public static GameControl Instance;

    //Declare public variables

    public GameObject TileWhite;
    public GameObject TileBlack;
    public int TileMin;
    public int TileMax;

    public int NumOfTiles;

    public BallController Ball;

    //Declare private variables
    private List<GameObject> TilesToDestroy = new List<GameObject>();
    private int StartingTile;

    // Use this for initialization
    void Start () {
		if(Instance == null)
        {
            Instance = this;
        }
        StartingTile = Random.Range(0,NumOfTiles);
        ArrangeTile();
    }

    public void ArrangeTile()
    {
        int temp = StartingTile + Random.Range(TileMin, TileMax+1);
        StartingTile = temp % NumOfTiles;

        for (int i = 0; i < NumOfTiles; i++)
        {
            // instantiate black tiles
            if (i != StartingTile)
            {
                TilesToDestroy.Add(Instantiate(TileBlack, transform.position, Quaternion.identity));
                TilesToDestroy[i].transform.eulerAngles = new Vector3(0, 0, (360 / NumOfTiles) * i);
            }
            // instantiate white tiles
            else
            {
                TilesToDestroy.Add(Instantiate(TileWhite, transform.position, Quaternion.identity));
                TilesToDestroy[i].transform.eulerAngles = new Vector3(0, 0, (360 / NumOfTiles) * i);
            }
        }        
    }

    public void DestroyTiles()
    {
        for (int i = 0; i < TilesToDestroy.Count; i++)
        {
            Destroy(TilesToDestroy[i]);            
        }
        TilesToDestroy.Clear();
    }

    public void Bounce()
    {
        Ball.Down = false;
        Ball.Up = true;
    }

    public int GetOrbitLevel()
    {
        return Ball.GetOrbitLevel();
    }

    public float[] GetOrbitHeights()
    {
        return Ball.OrbitHeights;
    }
}
