using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //Declare Public Variables
    public float AngularVelocity;

    //Declare Private Variables
    private Rigidbody2D rb;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = AngularVelocity;
	}
}
