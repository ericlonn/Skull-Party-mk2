using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public Color playerColor = Color.magenta;
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
        // if (isMovingRight)
        // {
        transform.Translate(Vector2.right * speedScale);
        // } else {
        //     transform.Translate(-Vector2.right * speedScale);
        // }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetInstanceID() != playerID)
        {
            Debug.Log("other: " + other.gameObject.GetInstanceID() + " | self:" + gameObject.GetInstanceID());
            Vector2 launchForce;
            if (isMovingRight) { launchForce = new Vector2(StunForceX, StunForceY); }
            else { launchForce = new Vector2(-StunForceX, StunForceY); }
            other.gameObject.GetComponent<Player>().TriggerStun(launchForce);

            GameObject newHitParticles = Instantiate(hitParticles, gameObject.transform.position, Quaternion.identity);

            Destroy(gameObject, 0f);
        }
    }
}
