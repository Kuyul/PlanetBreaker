using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //Declare public variables
    public Transform Ball;

    //Declare private variables
    private Camera Cam;
    private float CamOrigSize;
    private float CamTargetSize;
    private float CamDiff;
    private GameControl Gc;
    private int CurrentLevel;
    private int PreviousLevel;
    private bool GoingUp = false;
    private bool LevelReset = false;

    // Use this for initialization
    void Start()
    {
        Cam = Camera.main;
        CamOrigSize = 6.0f;
        Gc = GameControl.Instance;

        //Initialise Previous level
        PreviousLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float[] orbitHeights = Gc.GetOrbitHeights();
        //Get the current orbit level
        CurrentLevel = Gc.GetOrbitLevel();

        //Check if there was an increase in orbit level, if so, set goingup flag to true until it reaches the orbit height
        if(CurrentLevel > PreviousLevel)
        {
            GoingUp = true;
            LevelReset = false;
            var scale = orbitHeights[CurrentLevel] / orbitHeights[PreviousLevel];
            CamTargetSize = Cam.orthographicSize * scale;
            CamDiff = CamTargetSize - Cam.orthographicSize;
        }
        else if (CurrentLevel < PreviousLevel)
        {
            GoingUp = false;
            LevelReset = true;
        }

        //When the ball is going up up a orbit
        if (GoingUp)
        {
            Cam.orthographicSize += CamDiff / 20;

            if (Cam.orthographicSize >= CamTargetSize)
            {
                GoingUp = false;
            }
        }

        //Zoom in until it reaches the initial scale
        if (LevelReset)
        {
            if(Cam.orthographicSize > CamOrigSize)
            {
                Cam.orthographicSize -= 0.1f;
            }
            else
            {
                LevelReset = false;
            }
        }

        //TODO: When the ball falls in orbit
        PreviousLevel = CurrentLevel;
    }
}
