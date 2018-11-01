using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //Declare Public Variables
    public float StartingAngularVelocity;
    public float StopVelocity;
    public float AVFallIncrement;
    public float AVUpIncrement;
    public Rigidbody2D rb;

    //Declare Private Variables

    private float SlowTime;


	// Use this for initialization
	void Start () {
        SlowTime = 1f;
        rb.angularVelocity= StartingAngularVelocity;
	}

    private void Update()
    {
        SlowTime -= Time.deltaTime;

        if (SlowTime < 0)
        {
            rb.angularVelocity -= AVFallIncrement * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // 5 seconds to stop timer while ball is falling so it doesnt gameover half way through projectile
            SlowTime = 5;
        }

        if (rb.angularVelocity < StopVelocity)
        {
            GameControl.Instance.GameOver();
        }
    }

    public void ResetAngularVelocity()
    {
        SlowTime = 1f;
        rb.angularVelocity = StartingAngularVelocity;
    }

    public void AddAngularVelocity()
    {
        SlowTime = 1f;
        rb.angularVelocity += AVUpIncrement;
    }

}
