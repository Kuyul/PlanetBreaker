using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Declare Private variables
    private int Lvl;
    private Vector3 FallVector;
    private Vector3 UpVector;
    private bool ShieldActive = false;

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

            GameControl.Instance.circleInner.color = new Color32(255,122,61, 255);
            GameControl.Instance.circleOuter.color = new Color32(168,4,17, 109);
            GameControl.Instance.ovalOuter.color = new Color32(255, 118, 118, 255);
        }
        if (Lvl == 3)
        {
            GameControl.Instance.g0innertrail.SetActive(false);
            GameControl.Instance.g0outertrail.SetActive(false);
            GameControl.Instance.g3innertrail.SetActive(true);
            GameControl.Instance.g3outertrail.SetActive(true);

            GameControl.Instance.circleInner.color = new Color32(214,254,254, 255);
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
        if (collision.tag == "Alien1")
        {
            GameControl.Instance.Bounce();
            SetOrbitLevel(3);
            GameControl.Instance.AddJewel(5);
            collision.gameObject.SetActive(false);
        }
        if(collision.tag == "Alien2")
        {
            GameControl.Instance.Bounce();
            ShieldActive = true;
            Shield.SetActive(true);
            GameControl.Instance.AddJewel(5);
            collision.gameObject.SetActive(false);
        }

        //Sometimes this would trigger twice because the ball falls below the tiles and it hits the tile second the when it comes back up.
        if (Down)
        {
            //But the ball back into orbit if it hits a black tile
            if (collision.tag == "BlackTile")
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
            //If it hits a white tile, the tile changes to a Black Tile
            else if (collision.tag == "WhiteTile")
            {
                GameControl.Instance.ReduceHealth(Lvl + 1); //Reduce planet health
                SetOrbitLevel(0);
                GameControl.Instance.Bounce();
                GameObject temp = Instantiate(GameControl.Instance.pewhite, transform.position, Quaternion.identity);
                Destroy(temp, 2);
                player.ResetAngularVelocity();
                GameControl.Instance.ConvertTile(collision.gameObject);
                GameControl.Instance.SpawnAlien();

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
                GameControl.Instance.ReduceHealth(Lvl + 1); //Reduce planet health
                GameControl.Instance.Bounce();
                GameObject temp = Instantiate(GameControl.Instance.peyellow, transform.position, Quaternion.identity);
                Destroy(temp, 2);
                player.AddAngularVelocity();
                GameControl.Instance.ConvertTile(collision.gameObject);
                GameControl.Instance.SpawnAlien();

                if (Lvl + 1 < OrbitHeights.Length)
                {
                    SetOrbitLevel(Lvl + 1);
                }

            }
        }
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
    }
}
