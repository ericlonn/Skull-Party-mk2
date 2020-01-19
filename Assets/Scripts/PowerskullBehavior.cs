using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerskullBehavior : MonoBehaviour
{
    public GameObject playerCollected = null;
    public float collectedAnimLargeScale = 2f;
    public float collectedAnimLarge = .5f;
    public float collectedAnimSmall = .5f;
    public float collectedAlpha = .75f;
    public float bumpForceX = 1f;
    public float bumpForceY = 1f;

    Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

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

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && playerCollected == null && !other.gameObject.GetComponent<Player>().isPoweredUp)
        {
            playerCollected = other.gameObject;
            // GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            playerCollected.GetComponent<Player>().powerskullCount++;

            Color tmp = GetComponent<SpriteRenderer>().color;
            tmp.a = collectedAlpha;
            GetComponent<SpriteRenderer>().color = tmp;
            StartCoroutine("CollectedAnim");
        }
        else if (other.gameObject.CompareTag("Player") && playerCollected == null && other.gameObject.GetComponent<Player>().isPoweredUp)
        {
            float poweredPlayerDirection = Mathf.Sign(transform.position.x - other.gameObject.transform.position.x);
            Vector2 bumpVector = new Vector2(bumpForceX * poweredPlayerDirection, bumpForceY);
            GetComponent<Rigidbody2D>().AddForce(bumpVector, ForceMode2D.Impulse);
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

}
