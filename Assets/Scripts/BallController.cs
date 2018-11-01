using System.Collections;
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
    public Text DmgText;

    //Texts
    public GameObject NiceText;
    public GameObject ExtremeText;
    public GameObject MagnificentText;
    public GameObject TextSpawnPosition;

    //Declare Private variables
    private int Lvl;
    private Vector3 FallVector;
    private Vector3 UpVector;
    private bool ShieldActive = false;
    private bool Undying = false;

    private int reverseNumber=0;

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
            GameControl.Instance.alienhit.Play();
        }
        if(collision.tag == "Alien2")
        {
            GameControl.Instance.Bounce();
            ShieldActive = true;
            Shield.SetActive(true);
            GameControl.Instance.AddJewel(5);
            collision.gameObject.SetActive(false);
            GameControl.Instance.alienhit.Play();
        }
        if (collision.tag == "Alien3")
        {       
            reverseNumber += 1;
            GameControl.Instance.PlayerBall.localEulerAngles = new Vector3(0, 0, 180*reverseNumber);
            player.StartingAngularVelocity = -player.StartingAngularVelocity;
            player.rb.angularVelocity = player.StartingAngularVelocity;
            GameControl.Instance.Bounce();
            GameControl.Instance.AddJewel(5);
            collision.gameObject.SetActive(false);
            GameControl.Instance.alienhit.Play();
        }

        //Sometimes this would trigger twice because the ball falls below the tiles and it hits the tile second the when it comes back up.
        if (Down)
        {
            //If it hits a white tile, the tile changes to a Black Tile
            if (collision.tag == "WhiteTile")
            {
                var dmg = (int)Mathf.Pow(2, Lvl);
                GameControl.Instance.ReduceHealth(dmg); //Reduce planet health
                GameControl.Instance.Bounce();
                //StartCoroutine(UndyingTimer());
                if (Lvl >2)
                {
                    GameObject temp = Instantiate(GameControl.Instance.peblue, transform.position, Quaternion.identity);
                    Destroy(temp, 2);
                }
                SetOrbitLevel(0);
                GameObject temp2 = Instantiate(GameControl.Instance.pewhite, transform.position, Quaternion.identity);
                Destroy(temp2, 2);
                player.ResetAngularVelocity();
                GameControl.Instance.ConvertTile(collision.gameObject);
                GameControl.Instance.SpawnAlien();
                GameControl.Instance.IncrementTimer();
                GameControl.Instance.hits[Random.Range(0, GameControl.Instance.hits.Length)].Play();
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
                var dmg = (int)Mathf.Pow(2, Lvl);
                GameControl.Instance.ReduceHealth(dmg); //Reduce planet health
                GameControl.Instance.Bounce();
                //StartCoroutine(UndyingTimer());
                if (Lvl > 2)
                {
                    GameObject temp = Instantiate(GameControl.Instance.peblue, transform.position, Quaternion.identity);
                    Destroy(temp, 2);
                }

                GameObject temp2 = Instantiate(GameControl.Instance.peyellow, transform.position, Quaternion.identity);
                Destroy(temp2, 2);

                player.AddAngularVelocity();
                GameControl.Instance.ConvertTile(collision.gameObject);
                GameControl.Instance.SpawnAlien();
                GameControl.Instance.IncrementTimer();
                GameControl.Instance.hits[Random.Range(0, GameControl.Instance.hits.Length)].Play();

                if (Lvl + 1 < OrbitHeights.Length)
                {
                    SetOrbitLevel(Lvl + 1);
                }

            }
            else if (collision.tag == "BlackTile")
            {
                //StartCoroutine(BlackTile());
                if (!Undying)
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
        if(Lvl == 0)
        {
            DmgText.text = "";
        }
        else
        {
            var multi = (int)Mathf.Pow(2, Lvl);
            DmgText.text = "DMG x" + multi;
        }
    }

    //Set the ball to "Undying Mode", which prevents the ball from dying for 0.1 seconds after hitting a yellow or white tile
    IEnumerator UndyingTimer()
    {
        Undying = true;
        yield return new WaitForSeconds(0.1f);
        Undying = false;
    }

    //Wait 0.01 second to see whether the ball has also collided with a yellow or white tile, if so, perform the following logic.
    IEnumerator BlackTile()
    {
        yield return new WaitForSeconds(0.01f);
        
    }

    IEnumerator CreateText(int lvl)
    {
        GameObject obj;

        //if (lvl == 1)
        //{
            obj = Instantiate(NiceText, TextSpawnPosition.transform.position, Quaternion.identity);
        //}

        yield return new WaitForSeconds(0.9f);

        Destroy(obj);
    }
}
