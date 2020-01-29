using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerskullBehavior : MonoBehaviour
{
    public GameObject playerCollected = null;
    public GameObject spawnBurst;
    public float collectedAnimLargeScale = 2f;
    public float collectedAnimLarge = .5f;
    public float collectedAnimSmall = .5f;
    public float collectedAlpha = .75f;
    public float bumpForceX = 1f;
    public float bumpForceY = 1f;

    public bool ejected = false;
    public bool ejectedCRRunning = false;

    Rigidbody2D _rb;
    PlayerManager _playerMan;
    bool isCollectable = false;
    bool lastFrameColliding = false;

    // Start is called before the first frame update
    void Start()
    {
        _playerMan = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        _rb = GetComponent<Rigidbody2D>();
        Instantiate(spawnBurst, transform.position, Quaternion.identity);
        StartCoroutine("JustSpawnedTime");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCollected != null)
        {
            transform.position = playerCollected.transform.position;
        }

        if (_rb.velocity.x > bumpForceX)
        {
            _rb.velocity = new Vector2(bumpForceX, _rb.velocity.y);
        }

        if (_rb.velocity.y > bumpForceY)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, bumpForceY);
        }

    }

    private void LateUpdate()
    {
        if (ejected)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 300f));

            if (!ejectedCRRunning)
            {
                float randomXForceSign = Mathf.Sign(Random.Range(-1, 1));
                float randomXForce = Random.Range(100, 200) * randomXForceSign;

                StartCoroutine("EjectedFlyUp");
                GetComponent<Rigidbody2D>().AddForce(new Vector2(randomXForce, 0));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (isCollectable)
        {
            if (other.gameObject.CompareTag("Player") && playerCollected == null && !other.gameObject.GetComponent<Player>().isPoweredUp)
            {
                playerCollected = other.gameObject;
                // GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                playerCollected.GetComponent<Player>().powerskullCount++;
                playerCollected.GetComponent<Player>().score += 50;

                Color tmp = GetComponent<SpriteRenderer>().color;
                tmp.a = collectedAlpha;
                GetComponent<SpriteRenderer>().color = tmp;
                StartCoroutine("CollectedAnim");
                GameObject.Find("Sound Manager").GetComponent<PlaySound>().PlayClip(4, false);
                return;
            }
            else if (other.gameObject.CompareTag("Player") && playerCollected == null && other.gameObject.GetComponent<Player>().isPoweredUp)
            {
                float poweredPlayerDirection = Mathf.Sign(transform.position.x - other.gameObject.transform.position.x);
                Vector2 bumpVector = new Vector2(bumpForceX * poweredPlayerDirection, bumpForceY);
                GetComponent<Rigidbody2D>().AddForce(bumpVector, ForceMode2D.Impulse);
            }
            // GameObject.Find("Sound Manager").GetComponent<PlaySound>().PlayClip(3, true);
        }
    }

    IEnumerator CollectedAnim()
    {
            Vector3 collectedVector = new Vector3(collectedAnimLargeScale, collectedAnimLargeScale, collectedAnimLargeScale);
            LeanTween.scale(gameObject, collectedVector, collectedAnimLarge).setEase(LeanTweenType.easeInOutSine);
            yield return new WaitForSeconds(collectedAnimLarge);
            LeanTween.scale(gameObject, Vector3.zero, collectedAnimSmall).setEase(LeanTweenType.easeInOutSine);
            yield return new WaitForSeconds(collectedAnimSmall);

        Destroy(gameObject, 0f);
    }

    IEnumerator JustSpawnedTime()
    {
        foreach (GameObject player in _playerMan.playerObjects)
        {
            if (player != null)
            {
                Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), true);
            }
        }


        yield return new WaitForSeconds(.2f);
        isCollectable = true;

        foreach (GameObject player in _playerMan.playerObjects)
        {
            if (player != null)
            {
                Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), false);
            }
        }
    }

    IEnumerator EjectedFlyUp()
    {
        ejectedCRRunning = true;
        yield return new WaitForSeconds(.15f);
        ejectedCRRunning = false;
        ejected = false;
    }

}
