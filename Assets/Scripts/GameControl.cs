using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

    public static GameControl Instance;

    //Declare public variables

    public GameObject[] TileWhite;
    public GameObject TileBlack;
    public int TileMin;
    public int TileMax;

    public SpriteRenderer rend;
    public Sprite[] planets;

    public SpriteRenderer rendBackground;
    public Sprite[] backgrounds;

    public int NumOfTiles;

    public GameObject peyellow;
    public GameObject pewhite;

    public BallController Ball;

    public Animator g1;
    public Animator g2;
    public Animator g3p1;
    public Animator g3p2;

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

        rend.sprite = planets[Random.Range(0, planets.Length)];
        rendBackground.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
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
                TilesToDestroy.Add(Instantiate(TileWhite[Random.Range(0,TileWhite.Length)], transform.position, Quaternion.identity));
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

    public void GameOver()
    {
        SceneManager.LoadScene(0);
    }
}
