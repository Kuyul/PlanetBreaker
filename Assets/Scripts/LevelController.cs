using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    //Declare public variables
    public Rigidbody2D Planet;
    public Text HealthText;
    public GameObject[] AlienContainer;
    public float AlienAngularVelocity;

    public float planetSpinSpeed; //TODO: remove these later as we won't need it
    public int Health = 10;
    public int SpawnAlien = 1;

    //Level Properties
    private int HealthLeft = 10;

    // Use this for initialization
    void Start () {
        GenerateLevel(planetSpinSpeed, Health, SpawnAlien);
	}

    public void GenerateLevel(float planetSpin, int health, int alienCount)
    {
        Planet.angularVelocity = planetSpin;
        HealthLeft = health;
        HealthText.text = "HEALTH " + HealthLeft;
        GameObject obj = Instantiate(AlienContainer[Random.Range(0, AlienContainer.Length)]);
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.angularVelocity = AlienAngularVelocity;
    }

    public void ReduceHealth(int reduce)
    {
        HealthLeft -= reduce;
        HealthText.text = "HEALTH " + HealthLeft;

        if(HealthLeft <= 0)
        {
            GameControl.Instance.NextLevel();
        }
    }

    public void AlienEffect(GameObject obj)
    {
        if(obj.tag == "Alien1")
        {
            AlienEffect1();
        }//TODO: Include more alien effects
    }

    public void AlienEffect1()
    {

    }

    public void AlienEffect2()
    {

    }
}
