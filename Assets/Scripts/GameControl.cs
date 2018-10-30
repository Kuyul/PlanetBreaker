using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    public static GameControl Instance;

    //Declare public variables

    public GameObject TileWhite;
    public GameObject TileRed;
    public GameObject TileBlack;

    public int NumOfTiles;

    public BallController Ball;

    //Declare private variables
    private List<GameObject> TilesToDestroy = new List<GameObject>();
    private int CurrentWhiteTile;

    // Use this for initialization
    void Start () {
		if(Instance == null)
        {
            Instance = this;
        }
        CurrentWhiteTile = Random.Range(0, NumOfTiles);
        ArrangeTile();
    }

    public void ArrangeTile()
    {
        // WhiteTile is tile that would be white. Randomising starting white tile.
        int WhiteTile = Random.Range(0, NumOfTiles);

        while (CurrentWhiteTile == WhiteTile)
        {
            WhiteTile = Random.Range(0, NumOfTiles);
        }

        // setting below so that white tile doesnt spawn at its current position
        CurrentWhiteTile = WhiteTile;

        for (int i = 0; i < NumOfTiles; i++)
        {
            // instantiate black tiles
            if (i != WhiteTile)
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
