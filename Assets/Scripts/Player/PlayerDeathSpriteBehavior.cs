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
    public ParticleSystem _particles;

    // Start is called before the first frame update
    void Start()
    {
        _particles = transform.Find("Player Death Particles").GetComponent<ParticleSystem>();
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
        foreach (Collider2D overlapCol in deathPushbackCheck)
        {
            if (overlapCol.gameObject.CompareTag("Player"))
            {
                overlapCol.gameObject.GetComponent<MovementController>().SetVelocity(new Vector2(Mathf.Sign(overlapCol.transform.position.x - transform.position.x) * 30f, 12f));
            }
        }

        EjectPowerskull();
        _particles.Stop();

        if (GameObject.Find("PlayerManager").GetComponent<PlayerManager>().playerCount > 1)
        {
            StartCoroutine("DelayedDeath");
        } else {
            Color tmpColor = GetComponent<SpriteRenderer>().color;
            tmpColor.a = 0f;
            GetComponent<SpriteRenderer>().color = tmpColor;
            _particles.Stop();
        }

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
        LeanTween.move(gameObject, GameObject.Find("Powerskull Manager").GetComponent<PowerskullManager>().averagePlayerLocation, .7f).setEase(LeanTweenType.easeInOutQuad);
        yield return new WaitForSeconds(.7f);
        targetGroup.RemoveMember(transform);
        Destroy(gameObject);
    }
}
