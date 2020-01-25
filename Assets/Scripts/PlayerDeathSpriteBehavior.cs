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

    void Explode() {
        GameObject newSplatter = Instantiate(bloodSplatterObj, transform.position, Quaternion.identity);
        SpriteRenderer newSplatterExplosionSprite = newSplatter.transform.Find("blood splatter sprite").GetComponent<SpriteRenderer>();
        SpriteRenderer newSplatterBloodSprite = newSplatter.transform.Find("Death Explosion").GetComponent<SpriteRenderer>();

        newSplatterExplosionSprite.color = playerColor;
        newSplatterBloodSprite.color = playerColor;

        EjectPowerskull();
        targetGroup.RemoveMember(transform);
        Destroy(gameObject);    
    }

    void EjectPowerskull() {
        GameObject ejectedPowerskull = Instantiate(powerskullPrefab, transform.position, Quaternion.identity);
        ejectedPowerskull.GetComponent<PowerskullBehavior>().ejected = true;
    }
}
