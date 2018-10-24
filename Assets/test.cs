using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    //Declare Public Variables
    public float torq;
    public float force;
    public Transform testing;

 //   public float pushforce;
    private bool pushingforce;

    //Declare Private Variables
    private Rigidbody2D rb;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 testvelocity = new Vector2((testing.position.x - transform.position.x),(testing.position.y- transform.position.y));
        rb.velocity = Vector2.Perpendicular(testvelocity*force);
    }
}
