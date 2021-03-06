﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    //Declare Public variables
    public float FallSpeed;
    public float BounceSpeed;
    public float[] OrbitHeights;

    public bool Down = false;
    public bool Up = false;
    public bool Clickable = true;

    public PlayerController player;
    public GameObject Shield;

    //Emotes
    public GameObject[] Emotes;

    //Declare Private variables
    private int Lvl;
    private Vector3 FallVector;
    private Vector3 UpVector;
    private bool ShieldActive = false;

    private int reverseNumber = 0;
 
    // Use this for initialization
    void Start()
    {
        //Initialise orbit
        Lvl = 0;
        transform.localPosition = new Vector3(0, OrbitHeights[Lvl], 0); //Set the initial player position to the height of the first orbit layer

        FallVector = new Vector3(0, FallSpeed, 0);
        UpVector = new Vector3(0, BounceSpeed, 0);
    }

    private void Update()
    {
        // changing color of player and trailer according to current orbit
        if (Lvl == 0)
        {
            GameControl.Instance.g0innertrail.SetActive(true);
            GameControl.Instance.g0outertrail.SetActive(true);
            GameControl.Instance.g3innertrail.SetActive(false);
            GameControl.Instance.g3outertrail.SetActive(false);

            GameControl.Instance.circleInner.color = new Color32(255, 122, 61, 255);
            GameControl.Instance.circleOuter.color = new Color32(168, 4, 17, 109);
            GameControl.Instance.ovalOuter.color = new Color32(255, 118, 118, 255);
        }
        if (Lvl == 3)
        {
            GameControl.Instance.g0innertrail.SetActive(false);
            GameControl.Instance.g0outertrail.SetActive(false);
            GameControl.Instance.g3innertrail.SetActive(true);
            GameControl.Instance.g3outertrail.SetActive(true);

            GameControl.Instance.circleInner.color = new Color32(214, 254, 254, 255);
            GameControl.Instance.circleOuter.color = new Color32(214, 254, 254, 109);
            GameControl.Instance.ovalOuter.color = new Color32(1, 87, 255, 255);
        }

        if (Lvl == 1)
        {
            GameControl.Instance.g1.SetBool("fadein", true);
        }
        if (Lvl > 1)
        {
            GameControl.Instance.g2.SetBool("fadein", true);
        }
        if (Lvl > 2)
        {
            GameControl.Instance.g3p1.SetBool("fadein", true);
            GameControl.Instance.g3p2.SetBool("fadein", true);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 NewPosition = transform.localPosition;

        //while "Down" ball speeds down.
        if (Down)
        {
            NewPosition -= FallVector;
        }

        //while "Up" the ball speeds up.
        if (Up)
        {
            NewPosition += UpVector;
        }

        //At any time if the position of the ball is higher than the orbit, adjust to the current orbit's height
        if (NewPosition.y >= OrbitHeights[Lvl])
        {
            NewPosition = new Vector3(0, OrbitHeights[Lvl], 0);
            Up = false;
            Clickable = true;
        }

        transform.localPosition = NewPosition;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Sometimes this would trigger twice because the ball falls below the tiles and it hits the tile second the when it comes back up.
        if (Down)
        {
            if (collision.tag == "Alien1")
            {
                AlienCommonFunctions(collision);
                SetOrbitLevel(3);
            }
            if (collision.tag == "Alien2")
            {
                AlienCommonFunctions(collision);
                ShieldActive = true;
                Shield.SetActive(true);
            }
            if (collision.tag == "Alien3")
            {
                AlienCommonFunctions(collision);
                reverseNumber += 1;
                GameControl.Instance.PlayerBall.localEulerAngles = new Vector3(0, 0, 180 * reverseNumber);
                player.rb.angularVelocity = -player.rb.angularVelocity;
            }

            //If it hits a white tile, the tile changes to a Black Tile
            if (collision.tag == "WhiteTile")
            {
                TileCommonFunctions(collision);
                SetOrbitLevel(0);
                //Particle Effect
                GameObject temp2 = Instantiate(GameControl.Instance.pewhite, transform.position, Quaternion.identity);
                Destroy(temp2, 2);
                player.ResetAngularVelocity();
                //This if statement will always be true
                if (Lvl == 0)
                {
                    GameControl.Instance.g1.SetBool("fadein", false);
                    GameControl.Instance.g2.SetBool("fadein", false);
                    GameControl.Instance.g3p1.SetBool("fadein", false);
                    GameControl.Instance.g3p2.SetBool("fadein", false);
                }
            }
            //If it hits a yellow tile, break the tile and go up a level
            else if (collision.tag == "YellowTile")
            {
                TileCommonFunctions(collision);
                //Particle Effect
                GameObject temp2 = Instantiate(GameControl.Instance.peyellow, transform.position, Quaternion.identity);
                Destroy(temp2, 2);
                player.AddAngularVelocity();
                //Increment orbit height
                if (Lvl + 1 < OrbitHeights.Length)
                {
                    SetOrbitLevel(Lvl + 1);
                }

            }
            else if (collision.tag == "BlackTile")
            {

                if (ShieldActive)
                {
                    ShieldActive = false;
                    Shield.SetActive(false);
                    GameControl.Instance.Bounce();
                }
                else
                {
                    GameControl.Instance.Player.SetActive(false);
                    Instantiate(GameControl.Instance.pePlayerExplosion, transform.position, Quaternion.identity);
                    GameControl.Instance.GameOver();
                }
            }
            else if (collision.tag == "GreenTile")
            {
                TileCommonFunctions(collision);
                GameControl.Instance.AddJewel(1);
                GameObject temp2 = Instantiate(GameControl.Instance.pejewel, transform.position, Quaternion.identity);
                Destroy(temp2, 2);
            }
        }
    }


    //We always always want to group common functions ito one method for
    //1. its easy to maintain code
    //2. it looks much better (shorter code)
    //3. its easier to read code in the future

    //Group common tile functions for white/yellow/green
    private void TileCommonFunctions(Collider2D collision)
    {
        if (Lvl > 2)
        {
            GameObject temp = Instantiate(GameControl.Instance.peblue, transform.position, Quaternion.identity);
            Destroy(temp, 2);
            GameObject temp2 = Instantiate(GameControl.Instance.peblue2, transform.position, Quaternion.identity);
            Destroy(temp2, 2);
        }
        var dmg = (int)Mathf.Pow(2, Lvl);
        GameControl.Instance.ReduceHealth(dmg); //Reduce planet health
        GameControl.Instance.Bounce();
        GameControl.Instance.ConvertTile(collision.gameObject);
        GameControl.Instance.SpawnAlien();
        GameControl.Instance.hits[Random.Range(0, GameControl.Instance.hits.Length)].Play();
    }

    //Group common alien functions
    private void AlienCommonFunctions(Collider2D collision) {
        collision.gameObject.SetActive(false);
        GameControl.Instance.Bounce();
        GameControl.Instance.alienhit.Play();
    }

    public int GetOrbitLevel()
    {
        return Lvl;
    }

    public void SetOrbitLevel(int level)
    {
        Lvl = level;
        FallVector = new Vector3(0, FallSpeed, 0) * (OrbitHeights[Lvl] / OrbitHeights[0]);
        UpVector = new Vector3(0, BounceSpeed, 0) * (OrbitHeights[Lvl] / OrbitHeights[0]);

        for (int i = 0; i < Emotes.Length; i++)
        {
            if (i + 1 == Lvl) {
                Emotes[i].SetActive(true);
            }
            else
            {
                Emotes[i].SetActive(false);
            }
            
        }
    }
}
