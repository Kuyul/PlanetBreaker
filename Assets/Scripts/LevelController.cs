using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{

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

    public int Health = 10;
    public int SpawnAlien = 1;
    public int ShieldAlienPercentage = 10;
    public int PowerAlienPercentage = 15;
    public int RotateAlienPercentage = 20;

    //Level Properties
    private int HealthLeft = 10;
    private bool ShieldAlienbool = false;
    private bool PowerAlienbool = false;
    private bool RotateAlienbool = false;
    private int Total = 0;

    // Use this for initialization
    void Start()
    {
        int level = GetLevel();
        //Set health equal to level
        Health = level + 5;

        //Set Level Text
        Level.text = "" + level;

        float PlanetSpin = 0;

        //Planet starts to spin from stage 5 onwards
        if (level >= 5)
        {         
            PlanetSpin = level;
            if(level > 40)
            {
                PlanetSpin = 40;
            }
        }

        //Rotate Aliens can appear from stage 15 onwards
        if (level >= 15)
        {
            RotateAlienbool = true;
        }

        //Power aliens can appear from stage 25 onwards
        if (level >= 25)
        {
            PowerAlienbool = true;
        }

        //Power aliens can appear from stage 35 onwards
        if (level >= 35)
        {
            ShieldAlienbool = true;
        }

        GenerateLevel(PlanetSpin, Health, SpawnAlien);
    }

    //This method is called at the start of every level to configure level properties
    public void GenerateLevel(float planetSpin, int health, int alienCount)
    {
        if (Random.Range(0, 2) == 0)
        {
            Planet.angularVelocity = planetSpin;
        }
        else
        {
            Planet.angularVelocity = -planetSpin;
        }
        HealthLeft = health;
        HealthBar.maxValue = health;
        HealthBar.value = health;
        if (PowerAlienbool)
        {
            Total += PowerAlienPercentage;
        }

        if (ShieldAlienbool)
        {
            Total += ShieldAlienPercentage;
        }
        if (RotateAlienbool)
        {
            Total += RotateAlienPercentage;
        }
    }

    public void ReduceHealth(int reduce)
    {
        HealthLeft -= reduce;
        HealthBar.value = HealthLeft;

        if (HealthLeft <= 0)
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
        int i = Random.Range(1, 100);
        //default chance 10%
        if (i <= AlienChance)
        {
            //Total is the sum of all the available proportions of each alien
            int j = Random.Range(0, Total);
            GameObject obj;
            //If j is less than 20 (default) spawn rotate alien
            if (j > 0 && j < RotateAlienPercentage)
            {
                obj = Instantiate(RotateAlien);
                SetAlienProperties(obj);
            }
            //If j is greater than or equal to 20 (default) and less than 35 spawn power alien
            else if (j >= RotateAlienPercentage && j < RotateAlienPercentage + PowerAlienPercentage)
            {
                obj = Instantiate(PowerAlien);
                SetAlienProperties(obj);
            }
            //else spawn shield
            else if (j >= RotateAlienPercentage + PowerAlienPercentage)
            {
                obj = Instantiate(ShieldAlien);
                SetAlienProperties(obj);
            }
            
        }
    }

    private void SetAlienProperties(GameObject obj)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        float AlienSpeed = Random.Range(-30.0f, 30.0f);
        obj.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        rb.angularVelocity = AlienSpeed;
    }
}
