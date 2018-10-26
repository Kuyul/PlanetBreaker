using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    //Declare Public variables
    public float FallSpeed = 0.01f;
    public float BounceSpeed = 0.2f;
    public float[] OrbitHeights;

    //Declare Private variables
    private bool Down = false;
    private bool Up = false;
    private bool Clickable = true;
    private int Lvl;
    private Vector3 FallVector;
    private Vector3 UpVector;

    // Use this for initialization
    void Start()
    {
        //Initialise orbit
        Lvl = 0;
        transform.localPosition = new Vector3(0, OrbitHeights[Lvl], 0); //Set the initial player position to the height of the first orbit layer

        FallVector = new Vector3(0, FallSpeed, 0);
        UpVector = new Vector3(0, BounceSpeed, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 NewPosition = transform.localPosition;

        //We can only click mousebutton when the ball isn't coming down
        if (Clickable)
        {
            if (Input.GetMouseButton(0))
            {
                Down = true;
                Up = false;
                Clickable = false;
            }
        }

        //while "Down" ball speeds down.
        if (Down)
        {
            NewPosition = new Vector3(0,2.0f,0);
        }

        //while "Up" the ball speeds up.
        if (Up)
        {
            NewPosition += UpVector;
        }

        //At any time if the position of the ball is higher than the orbit, adjust to the current orbit's height
        if (transform.localPosition.y > OrbitHeights[Lvl])
        {
            NewPosition = new Vector3(0, OrbitHeights[Lvl], 0);
            Up = false;
        }

        transform.localPosition = NewPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Sometimes this would trigger twice because the ball falls below the tiles and it hits the tile second the when it comes back up.
        if (Down)
        {
            //But the ball back into orbit if it hits a black tile
            if (collision.tag == "BlackTile")
            {
                Lvl = 0;
                //Nothing happens
            }
            //Game Over if it hits a red tile
            else if (collision.tag == "RedTile")
            {
                //TODO: implement gameover
            }
            //If it hits a white tile, the tile changes to a Black Tile
            else if (collision.tag == "WhiteTile")
            {
                //collision.gameObject.SetActive(false);
                //TODO: implement white tile
            }
            //If it hits a yellow tile, break the tile and go up a level
            else if (collision.tag == "YellowTile")
            {
                //collision.gameObject.SetActive(false);
                if (Lvl + 1 < OrbitHeights.Length)
                    Lvl++;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //When planet is hit, put the ball back into orbit
        if (collision.tag == "Planet")
        {
            Down = false;
            Up = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Planet")
        {
            Clickable = true;
        }
    }

    public int GetOrbitLevel()
    {
        return Lvl;
    }
}
