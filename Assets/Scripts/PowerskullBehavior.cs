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

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (playerCollected != null)
        {
            transform.position = playerCollected.transform.position;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && playerCollected == null)
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
