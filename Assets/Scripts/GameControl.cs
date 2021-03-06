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
    public GameObject TileGreen;
    public int GreentileChance = 5; //Percentage

    public GameObject Menu;
    public GameObject InGame;
    public GameObject AnimBall;
    public GameObject Player;
    public GameObject Mutebutton;
    public GameObject UnMutebutton;
    public GameObject Healthbar;

    public Transform PlayerBall;

    public SpriteRenderer rend;
    public Sprite[] planets;

    public SpriteRenderer rendBackground;
    public Sprite[] backgrounds;

    public int NumOfTiles;
    public bool startgaming;
    
    public AudioSource alienhit;
    public AudioSource buttonpop;
    public AudioSource death;
    public AudioSource[] hits;
    public AudioSource ingame;
    public AudioSource intro;

    //Effects
    public GameObject peyellow;
    public GameObject pewhite;
    public GameObject peblue;
    public GameObject peblue2;
    public GameObject pejewel;
    public GameObject pePlayerExplosion;
    public GameObject peExplosion;
    public GameObject peTrail;
    
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
    public Text LevelText;
    public Text JewelTextMenu;

    //Declare Controllers
    public BallController Ball;
    public LevelController Level;
    public CameraControl CameraControl;

    public Animator g1;
    public Animator g2;
    public Animator g3p1;
    public Animator g3p2;
    public Animator ball;
    public Animator cam;

    //Declare private variables
    private List<GameObject> TilesToDestroy = new List<GameObject>();
    private int ActiveWhiteTiles;
    private int GreenTile = -1;

    // Use this for initialization

    private void Awake()
    {
        Application.targetFrameRate = 300;
        if (PlayerPrefs.GetInt("sound",1) == 0)
        {
            alienhit.volume = 0;
            buttonpop.volume = 0;
            for (int i = 0; i < hits.Length; i++)
            {
                hits[i].volume = 0;
            }
            ingame.volume = 0;
            intro.volume = 0;
            death.volume = 0;
        }    
    }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        //If level is continued, start the game right away without showing the main menu
        if (LevelContinue.Instance != null)
        {
            if (LevelContinue.Instance.levelIsContinued)
                StartGame();
        }

        //Set 1 white tile at the start
        ActiveWhiteTiles = 3;
        ArrangeTile();

        rend.sprite = planets[Random.Range(0, planets.Length)];
        rendBackground.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
        JewelText.text = "" + GetJewelCount();

        JewelTextMenu.text = PlayerPrefs.GetInt("Jewels", 0).ToString();
        LevelText.text = PlayerPrefs.GetInt("Level", 0).ToString();
    }

    private void Update()
    {
        // check if animation has finished playing
        if (AnimBall.activeInHierarchy)
        {
            if (ball.GetCurrentAnimatorStateInfo(0).IsName("BallMove") && ball.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                AnimBall.SetActive(false);
                Player.SetActive(true);
                Healthbar.SetActive(true);
                Destroy(CameraControl.GetComponent<Animator>());
            }
        }

        // update sound icon if button is pressed
        if (PlayerPrefs.GetInt("sound", 1) == 1)
        {
            Mutebutton.SetActive(false);
            UnMutebutton.SetActive(true);
        }
        else
        {
            Mutebutton.SetActive(true);
            UnMutebutton.SetActive(false);
        }
    }

    public void ArrangeTile()
    {
        List<int> WhiteTiles = new List<int>();

        //TODO: add a green tile by chance
        if (GreenTile == -1)
        {
            int chance = Random.Range(0, 100);
            if (chance < GreentileChance)
            {
                int index = Random.Range(0, NumOfTiles);
                while (WhiteTiles.Contains(index))
                {
                    index = Random.Range(0, NumOfTiles);
                }
                WhiteTiles.Add(index);
                GreenTile = index;
            }
        }
        else
        {
            WhiteTiles.Add(GreenTile);
        }

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
                GenerateTile(TileBlack, i);
            }
            // instantiate white tiles
            else
            {
                if (i == GreenTile)
                {
                    GenerateTile(TileGreen, i);
                }
                else
                {
                    GenerateTile(TileWhite[Random.Range(0, TileWhite.Length)], i);
                }
            }
        }
    }

    //Group tile generation functions
    private void GenerateTile(GameObject tile, int i)
    {
        GameObject obj = Instantiate(tile, transform.position, Quaternion.identity);
        obj.transform.SetParent(Planet);
        TilesToDestroy.Add(obj);
        TilesToDestroy[i].transform.eulerAngles = new Vector3(0, 0, (360 / NumOfTiles) * i + Planet.transform.rotation.eulerAngles.z);
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
        if (obj.tag == "WhiteTile"|| obj.tag == "YellowTile")
        {
            ActiveWhiteTiles--;
        }
        else if(obj.tag == "GreenTile")
        {
            GreenTile = -1;
        }

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
            ActiveWhiteTiles = Random.Range(1, 4);
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
        death.Play();
        ingame.Stop();
        LevelContinue.Instance.levelIsContinued = false;
        StartCoroutine(Timer(1f));
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        Level.IncrementLevel();
        AddJewel(5);
        LevelContinue.Instance.levelIsContinued = true;
        StartCoroutine(Timer(2.3f));
    }


    //LevelManager
    public void ReduceHealth(int reduce)
    {
        Level.ReduceHealth(reduce);
    }

    public void PauseGame()
    {
        buttonpop.Play();
        Time.timeScale = 0;
        PanelPause.SetActive(true);
        alienhit.Pause();
        buttonpop.Pause();
        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].Pause();
        }
        ingame.Pause();
        intro.Pause();
        death.Pause();
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        PanelPause.SetActive(false);
        alienhit.UnPause();
        buttonpop.UnPause();
        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].UnPause();
        }
        ingame.UnPause();
        intro.UnPause();
        death.UnPause();
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

    public void StartGame()
    {
        cam.SetBool("startgame", true);
        ball.SetBool("startgame", true);
        Menu.SetActive(false);
        InGame.SetActive(true);
        intro.Stop();
        ingame.Play();
    }

    public void SpawnAlien()
    {
        Level.SpawnAliens();
    }

    public void MuteAll()
    {
        alienhit.volume = 1;
        buttonpop.volume = 1;
        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].volume = 1;
        }
        ingame.volume = 1;
        intro.volume = 1;
        death.volume = 1;
        PlayerPrefs.SetInt("sound", 1);
    }

    public void UnMuteAll()
    {
        alienhit.volume = 0;
        buttonpop.volume = 0;
        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].volume = 0;
        }
        ingame.volume = 0;
        intro.volume = 0;
        death.volume = 0;
        PlayerPrefs.SetInt("sound", 0);        
    }
}
