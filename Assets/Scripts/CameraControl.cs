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
    private GameControl Gc;
    private int CurrentLevel;
    private int PreviousLevel;
    private bool GoingUp = false;
    private bool LevelReset = false;

    // Use this for initialization
    void Start()
    {
        Cam = Camera.main;
        CamOrigSize = Cam.orthographicSize;
        Gc = GameControl.Instance;

        //Initialise Previous level
        PreviousLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float[] orbitHeights = Gc.GetOrbitHeights();
        //Get the current ball height
        float ballHeight = Ball.localPosition.y;
        //Get the current orbit level
        CurrentLevel = Gc.GetOrbitLevel();

        //Check if there was an increase in orbit level, if so, set goingup flag to true until it reaches the orbit height
        if(CurrentLevel > PreviousLevel)
        {
            GoingUp = true;
        }else if (CurrentLevel < PreviousLevel)
        {
            LevelReset = true;
        }

        //When the ball is going up up a orbit
        if (GoingUp)
        {
            if (ballHeight > orbitHeights[CurrentLevel - 1])
            {
                float scale = Ball.localPosition.y / orbitHeights[0];
                Cam.orthographicSize = CamOrigSize * scale;
            }

            //When the ball has reached the orbit height, set goingup bool to false
            if(ballHeight == orbitHeights[CurrentLevel])
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
