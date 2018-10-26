using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    public static GameControl Instance;

    //Declare public variables
    public BallController Ball;

	// Use this for initialization
	void Start () {
		if(Instance == null)
        {
            Instance = this;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetOrbitLevel()
    {
        return Ball.GetOrbitLevel();
    }

    public float[] GetOrbitHeights()
    {
        return Ball.OrbitHeights;
    }
}
