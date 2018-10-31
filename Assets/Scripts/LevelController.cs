using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    //Declare public variables
    public Rigidbody2D Planet;
    public Text HealthText;

    public float planetSpinSpeed; //TODO: remove these later as we won't need it
    public int Health = 10;

    //Level Properties
    private int HealthLeft = 10;

    // Use this for initialization
    void Start () {
        GenerateLevel(planetSpinSpeed, Health);
	}

    public void GenerateLevel(float planetSpin, int Health)
    {
        Planet.angularVelocity = planetSpin;
        HealthLeft = Health;
        HealthText.text = "Health " + HealthLeft;
    }

    public void ReduceHealth(int reduce)
    {
        HealthLeft -= reduce;
        HealthText.text = "Health " + HealthLeft;

        if(HealthLeft <= 0)
        {
            GameControl.Instance.NextLevel();
        }
    }

    public void AlienEffect()
    {

    }

    public void AlienEffect1()
    {

    }

    public void AlienEffect2()
    {

    }
}
