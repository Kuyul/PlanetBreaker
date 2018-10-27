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

    // Use this for initialization
    void Start()
    {
        Cam = Camera.main;
        CamOrigSize = Cam.orthographicSize;
        Gc = GameControl.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        float ballHeight = Ball.localPosition.y;
        int lvl = Gc.GetOrbitLevel();
        float[] orbitHeights = Gc.GetOrbitHeights();
        if(lvl > 0)
        if (ballHeight > orbitHeights[lvl-1])
        {
            float scale = Ball.localPosition.y / orbitHeights[0];
            Cam.orthographicSize = CamOrigSize * scale;
        }
    }
}
