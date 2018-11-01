using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContinue : MonoBehaviour {

    public static LevelContinue Instance;

    //Declare public variables
    public bool levelIsContinued = false;

    // Use this for initialization
    void Start () {
        if (Instance == null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }
	
}
