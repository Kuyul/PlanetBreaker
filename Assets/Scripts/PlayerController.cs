using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //Declare Public Variables
    public float AngularVelocity = 30.0f;
    public float StopVelocity = 10.0f;

    //Declare Private Variables
    private Rigidbody2D rb;
    private float SlowTime = 1.0f;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = AngularVelocity;
	}

    private void Update()
    {
        if (SlowTime > 0)
        {
            SlowTime -= Time.deltaTime;
        }
        else
        {
            rb.angularVelocity -= 1.0f;
        }

        if (Input.GetMouseButtonDown(0))
        {
            SlowTime = 1.0f;
            rb.angularVelocity = AngularVelocity;
        }

        if (rb.angularVelocity < StopVelocity)
        {
            GameControl.Instance.GameOver();
        }
    }
}
