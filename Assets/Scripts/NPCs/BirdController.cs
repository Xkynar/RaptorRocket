using UnityEngine;
using System.Collections;

public class BirdController : MonoBehaviour {

    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private float minFlightSpeed = 3f;
    [SerializeField] private float maxFlightSpeed = 6f;
    [SerializeField] private float minFlightTime = 1f;
    [SerializeField] private float maxFlightTime = 3f;

    private bool flying = false;

    //Components
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
	}
	
	void FixedUpdate ()
    {
        if (flying)
            return;

        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, detectionMask);

        if (hit != null)
        {
            flying = true;
            animator.SetBool("Flying", true);

            //Calculate direction to fly
            float direction = Mathf.Sign(transform.position.x - hit.transform.position.x);
            sr.flipX = direction > 0;

            //Fly
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(direction * Random.Range(minFlightSpeed, maxFlightSpeed), Random.Range(minFlightSpeed, maxFlightSpeed));

            //Land
            StartCoroutine(Land());
        }
	}

    IEnumerator Land()
    {
        yield return new WaitForSeconds(Random.Range(minFlightTime, maxFlightTime));

        rb.velocity /= 2f;
        rb.gravityScale = 1f;
        flying = false;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        animator.SetBool("Flying", false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }
}
