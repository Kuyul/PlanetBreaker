using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{

    public static GameControl Instance;

    //Declare public variables
    public Transform Planet;
    public GameObject[] TileWhite;
    public GameObject TileBlack;

    public SpriteRenderer rend;
    public Sprite[] planets;

    public SpriteRenderer rendBackground;
    public Sprite[] backgrounds;

    public int NumOfTiles;

    //Effects
    public GameObject peyellow;
    public GameObject pewhite;
    public GameObject pePlayerExplosion;
    public GameObject player;
    public SpriteRenderer circleInner;
    public SpriteRenderer circleOuter;
    public SpriteRenderer ovalOuter;

    public GameObject g3innertrail;
    public GameObject g3outertrail;
    public GameObject g0innertrail;
    public GameObject g0outertrail;

    //Declare Controllers
    public BallController Ball;
    public LevelController Level;

    public Animator g1;
    public Animator g2;
    public Animator g3p1;
    public Animator g3p2;

    //Declare private variables
    private List<GameObject> TilesToDestroy = new List<GameObject>();
    private int ActiveWhiteTiles;

    // Use this for initialization
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //Set 1 white tile at the start
        ActiveWhiteTiles = 1;
        ArrangeTile();

        rend.sprite = planets[Random.Range(0, planets.Length)];
        rendBackground.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
    }

    public void ArrangeTile()
    {
        List<int> WhiteTiles = new List<int>();

        //Set a random location for each white tile
        for (int i = 0; i < ActiveWhiteTiles; i++)
        {
            int index = Random.Range(0, NumOfTiles);
            while (WhiteTiles.Contains(index))
            {
                index = Random.Range(0, NumOfTiles);
            }
            WhiteTiles.Add(index);
        }

        for (int i = 0; i < NumOfTiles; i++)
        {
            // instantiate black tiles
            if (!WhiteTiles.Contains(i))
            {
                GameObject obj = Instantiate(TileBlack, transform.position, Quaternion.identity);
                obj.transform.SetParent(Planet);
                TilesToDestroy.Add(obj);
                TilesToDestroy[i].transform.eulerAngles = new Vector3(0, 0, (360 / NumOfTiles) * i);
            }
            // instantiate white tiles
            else
            {
                GameObject obj = Instantiate(TileWhite[Random.Range(0, TileWhite.Length)], transform.position, Quaternion.identity);
                obj.transform.SetParent(Planet);
                TilesToDestroy.Add(obj);
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

    public void ConvertTile(GameObject obj)
    {
        GameObject parentobj = obj.transform.parent.gameObject;
        ActiveWhiteTiles--;

        if (ActiveWhiteTiles >= 1)
        {
            var newobj = Instantiate(TileBlack, transform.position, Quaternion.identity);
            newobj.transform.eulerAngles = obj.transform.eulerAngles;
            TilesToDestroy.Add(newobj);
            TilesToDestroy.Remove(parentobj);
            Destroy(parentobj);
        }
        else
        {
            DestroyTiles();
            ActiveWhiteTiles = Random.Range(1, 3);
            ArrangeTile();
        }
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
        StartCoroutine("timer");
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void ReduceHealth(int reduce)
    {
        Level.ReduceHealth(reduce);
    }
}
