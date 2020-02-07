using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TossableObject : MonoBehaviour
{
    public float tossForce = 10f;
    public float tossRotationMultiplier = 50f;
    public float gravityMultiplier = 20f;
    public bool isTossed = false;
    public float stunForceX = 30f;
    public float stunForceY = 10f;
    public float wallCheckDistance = .2f;
    public GameObject tosser;
    public Vector2 tossDirection = Vector2.zero;
    public BoxCollider2D _collider;
    public LayerMask groundLayer;
    public ParticleSystem impactParticles;
    public GameObject spawnBurst;

    public GameObject powerskullObj;
    public float psLaunchSpeedX = 30f;
    public float psLaunchSpeedY = 10f;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        Instantiate(spawnBurst, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

        if (!_collider.IsTouchingLayers(groundLayer) && !isTossed)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }

        if (isTossed)
        {
            transform.Translate(tossDirection * tossForce * Time.deltaTime);

            Collider2D[] wallCheckHit;

            if (transform.position.x > 100 || transform.position.x < -100)
            {
                Destroy(gameObject);
            }

            wallCheckHit = Physics2D.OverlapBoxAll(transform.position, new Vector2(_collider.size.x, _collider.size.y * .8f), 0f);
            bool hasCollidedGround = false;
            bool hasCollidedPlayer = false;
            List<GameObject> collidedPlayers = new List<GameObject>();


            foreach (Collider2D overlapCollider in wallCheckHit)
            {
                if (overlapCollider.gameObject.CompareTag("Ground"))
                {
                    hasCollidedGround = true;
                }

                if (overlapCollider.gameObject.CompareTag("Player") && overlapCollider.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                {
                    hasCollidedPlayer = true;
                    collidedPlayers.Add(overlapCollider.gameObject);
                }
            }

            if (hasCollidedGround || hasCollidedPlayer)
            {
                Instantiate(impactParticles, transform.position, Quaternion.identity);
                SpawnPowerskull();
                Destroy(gameObject);
            }

            if (hasCollidedPlayer)
            {
                foreach (GameObject collidedPlayer in collidedPlayers)
                {
                    collidedPlayer.gameObject.GetComponent<Player>().TriggerStun(new Vector2(Mathf.Sign(tossDirection.x) * stunForceX, stunForceY), false);
                }
            }
        }
    }

    public void TriggerHit(float hitDirection, GameObject passedTosser)
    {
        isTossed = true;
        tossDirection = new Vector2(hitDirection, 0);
        tosser = passedTosser;
        GameObject.Find("Sound Manager").GetComponent<PlaySound>().PlayClip(1, true, transform.position);
    }

    private void OnCollisionStay2D(Collision2D other)
    {

        if (!isTossed && other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<Player>().isStunned && other.gameObject.GetInstanceID() != tosser.GetInstanceID())
        {
            TriggerHit(Mathf.Sign(transform.position.x - other.gameObject.transform.position.x), other.gameObject);
        }

    }

    private void SpawnPowerskull()
    {
        GameObject newPowerskull = Instantiate(powerskullObj, transform.position, Quaternion.identity);

        float randomXForce = -Mathf.Sign(tossDirection.x) * psLaunchSpeedX;
        float randomYForce = psLaunchSpeedY;
        newPowerskull.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomXForce, randomYForce), ForceMode2D.Impulse);

    }

}
