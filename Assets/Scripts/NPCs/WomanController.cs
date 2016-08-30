using UnityEngine;
using System.Collections;

public class WomanController : MonoBehaviour {

    [SerializeField] private float detectionRadius = 5;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private float runForce = 10f;
    [SerializeField] private float maxRunSpeed = 4f;
    [SerializeField] private float minRunTime = 1f;
    [SerializeField] private float maxRunTime = 3f;

    private bool running = false;

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
        if (running)
            return;

        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, detectionMask);

        if (hit != null)
        {
            running = true;
            animator.SetBool("Running", true);

            //Run
            StartCoroutine(RunFrom(hit.transform));
        }
    }

    IEnumerator RunFrom(Transform entity)
    {
        float elapsedTime = 0;
        float runTime = Random.Range(minRunTime, maxRunTime);

        while (elapsedTime < runTime)
        {
            //Calculate direction to run
            float direction = Mathf.Sign(transform.position.x - entity.transform.position.x);

            //Check if we looped the map
            if (Vector3.Distance(entity.transform.position, transform.position) > 50f)
            {
                direction *= -1f;
            }

            sr.flipX = direction < 0;

            if(rb.velocity.magnitude < maxRunSpeed)
                rb.AddForce(Vector2.right * runForce * direction);

            //rb.velocity = Vector2.right * Random.Range(minRunSpeed, maxRunSpeed) * direction;

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        rb.velocity = Vector2.zero;
        running = false;
        animator.SetBool("Running", false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }
}
