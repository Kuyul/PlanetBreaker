using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    //Declare public variables
    public Rigidbody2D Planet;
    public Slider HealthBar;
    public Text Level;
    public GameObject ShieldAlien;
    public GameObject PowerAlien;
    public float AlienAngularVelocity;

    public float planetSpinSpeed; //TODO: remove these later as we won't need it
    public int Health = 10;
    public int SpawnAlien = 1;

    //Level Properties
    private int HealthLeft = 10;
    private bool ShieldAlienbool = false;
    private bool PowerAlienbool = false;

    // Use this for initialization
    void Start () {
        int level = GetLevel();
        //Set health equal to level
        Health = level;

        //Set Level Text
        Level.text = "" + level;

        float PlanetSpin = 0;
        if (level >= 21)
        {
            PowerAlienbool = true;
        }

        if(level >= 26)
        {
            PlanetSpin = planetSpinSpeed;
        }

        if(level >= 31)
        {
            ShieldAlienbool = true;
        }

        if (level >= 41) {
            PlanetSpin = planetSpinSpeed * 2;
        }

        GenerateLevel(PlanetSpin, Health, SpawnAlien);
    }

    public void GenerateLevel(float planetSpin, int health, int alienCount)
    {
        Planet.angularVelocity = planetSpin;
        HealthLeft = health;
        HealthBar.maxValue = health;
        HealthBar.value = health;
        if (PowerAlienbool)
        {
            GameObject obj = Instantiate(PowerAlien);
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            rb.angularVelocity = AlienAngularVelocity;
        }

        if (ShieldAlienbool)
        {
            GameObject obj = Instantiate(ShieldAlien);
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            rb.angularVelocity = AlienAngularVelocity;
        }
        
    }

    public void ReduceHealth(int reduce)
    {
        HealthLeft -= reduce;
        HealthBar.value = HealthLeft;

        if(HealthLeft <= 0)
        {
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
}
