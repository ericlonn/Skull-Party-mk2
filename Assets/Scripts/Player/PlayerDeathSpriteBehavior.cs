using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerDeathSpriteBehavior : MonoBehaviour
{

    public GameObject bloodSplatterObj;
    public Color playerColor = Color.white;
    public CinemachineTargetGroup targetGroup;
    public GameObject powerskullPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Explode()
    {
        GameObject newSplatter = Instantiate(bloodSplatterObj, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        SpriteRenderer newSplatterExplosionSprite = newSplatter.transform.Find("blood splatter sprite").GetComponent<SpriteRenderer>();
        SpriteRenderer newSplatterBloodSprite = newSplatter.transform.Find("Death Explosion").GetComponent<SpriteRenderer>();

        newSplatterExplosionSprite.color = playerColor;
        newSplatterBloodSprite.color = playerColor;

        Collider2D[] deathPushbackCheck = Physics2D.OverlapCircleAll(transform.position, 3f);
        foreach (Collider2D overlapCol in deathPushbackCheck) {
            if (overlapCol.gameObject.CompareTag("Player")) {
                overlapCol.gameObject.GetComponent<MovementController>().SetVelocity(new Vector2(Mathf.Sign(overlapCol.transform.position.x - transform.position.x) * 30f, 12f));
                Debug.Log(overlapCol.gameObject.name + " caught in the explosion");
            }
        }

        EjectPowerskull();
        
        StartCoroutine("DelayedDeath");
    }

    void EjectPowerskull()
    {
        GameObject ejectedPowerskull = Instantiate(powerskullPrefab, transform.position, Quaternion.identity);
        ejectedPowerskull.GetComponent<PowerskullBehavior>().ejected = true;
    }

    IEnumerator DelayedDeath()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(.4f);
        LeanTween.move(gameObject, GameObject.Find("Powerskull Manager").GetComponent<PowerskullManager>().averagePlayerLocation, .7f);
        yield return new WaitForSeconds(.7f);
        targetGroup.RemoveMember(transform);
        Destroy(gameObject);
    }
}
