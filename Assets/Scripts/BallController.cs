using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    //Declare Public variables
    public float OrbitHeight;
    public float PushForce;

    //Declare Private variables
    private Rigidbody2D rb;
    private bool test;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.transform.localPosition = new Vector2(0, OrbitHeight);      
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            test = true;
            //rb.AddForce(-Vector2.up*PushForce);
            //rb.MovePosition(Vector2.up * PushForce);
            Debug.Log("true");
        }
        if (test)
        {
            AddForce();
        }
	}
    private void AddForce()
    {
        rb.velocity = Vector2.up * PushForce;
        test = false;
        Debug.Log("turn");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Planet")
        {
            //rb.AddForce(Vector2.up * PushForce);
        }
    }
}
