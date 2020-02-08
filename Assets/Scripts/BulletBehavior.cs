using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public Color playerColor = Color.magenta;
    public GameObject playerResponsible;
    public bool isMovingRight;
    public float playerID;
    public float speedScale = 2f;
    public float StunForceX = 30f;
    public float StunForceY = 10f;
    public GameObject hitParticles;

    private SpriteRenderer bulletSprite;

    void Start()
    {
        bulletSprite = transform.Find("bullet sprite").GetComponent<SpriteRenderer>();
        playerColor.a = 1;
        bulletSprite.color = playerColor;



        if (isMovingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }


        gameObject.transform.localScale = new Vector2(.5f, 1);
        LeanTween.scaleX(gameObject, 1f, .2f);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Find("bullet sprite").GetComponent<SpriteRenderer>().color = playerColor;
        // if (isMovingRight)
        // {

        // } else {
        //     transform.Translate(-Vector2.right * speedScale);
        // }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * speedScale * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetInstanceID() != playerID)
        {
            Vector2 launchForce;
            if (isMovingRight) { launchForce = new Vector2(StunForceX, StunForceY); }
            else { launchForce = new Vector2(-StunForceX, StunForceY); }
            if (!other.gameObject.GetComponent<Player>().isStunned)
            {
                other.gameObject.GetComponent<Player>().health--;
                if (other.gameObject.GetComponent<Player>().health <= 0)
                {
                    other.gameObject.GetComponent<Player>().killedBy = playerResponsible;
                } else {
                    playerResponsible.GetComponent<Player>().score += 100;
                }
                other.gameObject.GetComponent<Player>().TriggerStun(launchForce, true);
                
            }



            GameObject newHitParticles = Instantiate(hitParticles, gameObject.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
            newHitParticles.GetComponent<SpriteRenderer>().color = playerColor;

            GameObject.Find("Sound Manager").GetComponent<PlaySound>().PlayClip(0, false);
            Destroy(gameObject, 0f);
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            GameObject newHitParticles = Instantiate(hitParticles, gameObject.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
            newHitParticles.GetComponent<SpriteRenderer>().color = playerColor;
            GameObject.Find("Sound Manager").GetComponent<PlaySound>().PlayClip(0, false);
            Destroy(gameObject, 0f);

        }
    }
}
