using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    //Declare public variables
    public Rigidbody2D Planet;

    public float planetSpinSpeed; //TODO: remove this later as we won't need it

	// Use this for initialization
	void Start () {
        GenerateLevel(planetSpinSpeed);
	}

    private void GenerateLevel(float planetSpin)
    {
        Planet.angularVelocity = planetSpin;
    }
}
