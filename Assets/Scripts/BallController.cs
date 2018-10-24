using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    //Declare Public variables
    public float OrbitHeight1 = 2.5f;
    public float FallStep = 0.1f;

    //Declare Private variables
    private float CurrentHeight = 0;
    private bool Down = false;

	// Use this for initialization
	void Start () {
        CurrentHeight = OrbitHeight1;
        transform.localPosition = new Vector3(0, CurrentHeight, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetMouseButton(0))
        {
            Down = true;
        }

        //User clicked, ball speeds down.
        if (Down)
        {
            CurrentHeight -= FallStep;
            transform.localPosition = new Vector3(0, CurrentHeight, 0);
        }
        else
        {
            if (CurrentHeight < OrbitHeight1)
            {
                CurrentHeight += FallStep;
                if(CurrentHeight > OrbitHeight1)
                {
                    CurrentHeight = OrbitHeight1;
                }
                transform.localPosition = new Vector3(0, CurrentHeight, 0);
            }
        }
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        //When planet is hit, put the ball back into orbit
        if(collision.tag == "Planet")
        {
            Down = false;
        }
    }
}
