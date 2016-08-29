using UnityEngine;
using System.Collections;

public class PDogController : MonoBehaviour {


    [SerializeField] private float detectionRadius = 5;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private float minRunSpeed = 3f;
    [SerializeField] private float maxRunSpeed = 6f;
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

            //Calculate direction to run
            float direction = Mathf.Sign(transform.position.x - hit.transform.position.x);
            sr.flipX = direction < 0;

            //Run
            StartCoroutine(Running(direction));
        }
    }

    IEnumerator Running(float direction)
    {
        float elapsedTime = 0;
        float runTime = Random.Range(minRunTime, maxRunTime);

        while (elapsedTime < runTime)
        {
            rb.velocity = Vector2.right * Random.Range(minRunSpeed, maxRunSpeed) * direction;

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
