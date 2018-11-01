using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    //Declare public variables
    public Rigidbody2D Planet;
    public Slider HealthBar;
    public Text Level;
    public GameObject RotateAlien;
    public GameObject ShieldAlien;
    public GameObject PowerAlien;
    public GameObject PlanetBase;
    public float AlienAngularVelocity;
    public int AlienChance = 10;

    public float planetSpinSpeed; //TODO: remove these later as we won't need it
    public int Health = 10;
    public int SpawnAlien = 1;

    //Level Properties
    private int HealthLeft = 10;
    private bool ShieldAlienbool = false;
    private bool PowerAlienbool = false;
    private bool RotateAlienbool = false;
    private List<GameObject> Aliens = new List<GameObject>();

    // Use this for initialization
    void Start () {
        int level = GetLevel();
        //Set health equal to level
        Health = level;

        //Set Level Text
        Level.text = "" + level;

        float PlanetSpin = 0;

        //testing . . . . . . alien3 appear from lev 2

        if (level >= 2)
        {
            RotateAlienbool = true;
        }

        //Power Aliens can appear from stage 21 onwards
        if (level >= 21)
        {
            PowerAlienbool = true;
        }

        //Planet starts to spin from stage 26 onwards
        if(level >= 26)
        {
            planetSpinSpeed = Random.Range(-30.0f, 30.0f);
            PlanetSpin = planetSpinSpeed;
        }

        //Shield aliens can appear from stage 31 onwards
        if(level >= 31)
        {
            ShieldAlienbool = true;
        }

        GenerateLevel(PlanetSpin, Health, SpawnAlien);
    }

    //This method is called at the start of every level to configure level properties
    public void GenerateLevel(float planetSpin, int health, int alienCount)
    {
        Planet.angularVelocity = planetSpin;
        HealthLeft = health;
        HealthBar.maxValue = health;
        HealthBar.value = health;
        if (PowerAlienbool)
        {
            Aliens.Add(PowerAlien);
        }

        if (ShieldAlienbool)
        {
            Aliens.Add(ShieldAlien);
        }
        if(RotateAlienbool)
        {
            Aliens.Add(RotateAlien);
        }
    }

    public void ReduceHealth(int reduce)
    {
        HealthLeft -= reduce;
        HealthBar.value = HealthLeft;

        if(HealthLeft <= 0)
        {
            PlanetBase.SetActive(false);
            Instantiate(GameControl.Instance.peExplosion, PlanetBase.transform.position, Quaternion.identity);
            Instantiate(GameControl.Instance.peTrail, PlanetBase.transform.position, Quaternion.identity);
            GameControl.Instance.NextLevel();
        }
    }

    public int GetLevel()
    {
        return PlayerPrefs.GetInt("Level", 1);
    }

    public void IncrementLevel()
    {
        int currentLevel = GetLevel();
        PlayerPrefs.SetInt("Level", currentLevel + 1);
    }

    public void SpawnAliens()
    {
        int i = Random.Range(1,100);
        //default chance 10%
        if(i <= AlienChance)
        {
            if (Aliens.Count > 0)
            {
                int j = Random.Range(0, Aliens.Count);
                GameObject obj = Instantiate(Aliens[j]);
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                float AlienSpeed = Random.Range(-30.0f, 30.0f);
                rb.angularVelocity = AlienSpeed;
            }
        }
    }
}
