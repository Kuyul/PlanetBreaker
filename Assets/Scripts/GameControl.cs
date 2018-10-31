﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public GameObject PanelPause;
    //Jewel
    public Text JewelText;

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
        JewelText.text = "" + GetJewelCount();
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

        //If there are other active white tiles, destroy the one hit by the ball and leave fill the space with a black tile.
        if (ActiveWhiteTiles >= 1)
        {
            var newobj = Instantiate(TileBlack, transform.position, Quaternion.identity);
            newobj.transform.SetParent(Planet);
            newobj.transform.eulerAngles = obj.transform.eulerAngles;
            TilesToDestroy.Add(newobj);
            TilesToDestroy.Remove(parentobj);
            Destroy(parentobj);
        }
        //If there are no more active white tiles, re-generate them
        else
        {
            DestroyTiles();
            ActiveWhiteTiles = Random.Range(1, 3);
            ArrangeTile();
        }
    }

    //Not sure why bounce would be in Gamecontrol
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
        StartCoroutine("Timer");
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        AddJewel(10);
        SceneManager.LoadScene(0);
    }


    //LevelManager
    public void ReduceHealth(int reduce)
    {
        Level.ReduceHealth(reduce);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        PanelPause.SetActive(true);
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        PanelPause.SetActive(false);
    }

    //Jewel functions
    public int GetJewelCount()
    {
        return PlayerPrefs.GetInt("Jewels", 0);
    }

    public void AddJewel(int add)
    {
        int count = GetJewelCount();
        int newCount = count + add;
        PlayerPrefs.SetInt("Jewels", newCount);
        JewelText.text = "" + newCount;
    }
}
