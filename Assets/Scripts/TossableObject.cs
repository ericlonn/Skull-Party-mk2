using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TossableObject : MonoBehaviour
{
    public float tossForce = 10f;
    public float tossRotationMultiplier = 50f;
    public float gravityMultiplier = 10f;
    public bool isTossed = false;
    public float stunForceX = 30f;
    public float stunForceY = 10f;
    public float wallCheckDistance = .2f;
    public GameObject tosser;
    public Vector2 tossDirection = Vector2.zero;
    public BoxCollider2D _collider;
    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_collider.IsTouchingLayers(groundLayer) && !isTossed)
        {
            MoveToFloor();
        }

        if (isTossed)
        {
            transform.Translate(tossDirection * tossForce * Time.deltaTime);

            Vector2 wallCheckOrigin;
            RaycastHit2D wallCheckHit;
            if (tossDirection.x > 0) {
                wallCheckOrigin = new Vector2(transform.position.x + _collider.bounds.extents.x, transform.position.y);
                wallCheckHit = Physics2D.Raycast(wallCheckOrigin, Vector2.right, wallCheckDistance);
            } else {
                wallCheckOrigin = new Vector2(transform.position.x - _collider.bounds.extents.x, transform.position.y);
                wallCheckHit = Physics2D.Raycast(wallCheckOrigin, -Vector2.right, wallCheckDistance);
            }

            if (wallCheckHit.collider.gameObject.CompareTag("Ground")) {
                Destroy(gameObject);
            }
        }
    }

    public void TriggerHit(float hitDirection, GameObject passedTosser)
    {
        isTossed = true;
        tossDirection = new Vector2(hitDirection, 0);
        tosser = passedTosser;
    }

    void MoveToFloor()
    {
        transform.Translate(Vector2.down * Time.deltaTime * gravityMultiplier);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isTossed && other.gameObject.CompareTag("Player") && other.gameObject.GetInstanceID() != tosser.GetInstanceID())
        {
            if (tossDirection.x > 1)
            {
                other.gameObject.GetComponent<Player>().TriggerStun(new Vector2(-stunForceX, stunForceY));
            } else {
                other.gameObject.GetComponent<Player>().TriggerStun(new Vector2(stunForceX, stunForceY));
            }

            Destroy(gameObject);
        }

        if (!isTossed && other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<Player>().isStunned && other.gameObject.GetInstanceID() != tosser.GetInstanceID()) {
            TriggerHit(Mathf.Sign(transform.position.x - other.gameObject.transform.position.x), other.gameObject);
        }

    }

}
