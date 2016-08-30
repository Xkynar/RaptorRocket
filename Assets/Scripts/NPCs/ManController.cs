using UnityEngine;
using System.Collections;

public class ManController : MonoBehaviour {

    [SerializeField] private float runRadius = 3f;
    [SerializeField] private float attackRadius = 5f;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private float shootDelay = 0.5f;
    [SerializeField] private float rechargeDelay = 0.5f;
    [SerializeField] private float runForce = 10f;
    [SerializeField] private float maxRunSpeed = 4f;
    [SerializeField] private float minRunTime = 1f;
    [SerializeField] private float maxRunTime = 3f;

    //Shooting
    [SerializeField] private GameObject spearPrefab;
    [SerializeField] private float minSpearForce = 300f;
    [SerializeField] private float maxSpearForce = 500f;

    //Audio
    [SerializeField] private AudioClip[] spearSounds;

    private bool running = false;
    private bool attacking = false;

    //Components
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate ()
    {
        if (running || attacking)
            return;

        //Check if we are close enough to run
        Collider2D runHit = Physics2D.OverlapCircle(transform.position, runRadius, detectionMask);

        if (runHit != null)
        {
            running = true;
            animator.SetBool("Running", true);

            //Run
            StartCoroutine(RunFrom(runHit.transform));
        }
        else ///If we are far enough not to run, see if we are close enough to attack
        {
            Collider2D attackHit = Physics2D.OverlapCircle(transform.position, attackRadius, detectionMask);

            if (attackHit != null)
            {
                attacking = true;
                animator.SetTrigger("Shoot");

                //Attack
                StartCoroutine(Shoot(attackHit.transform));
            }
        }
    }

    IEnumerator RunFrom(Transform entity)
    {
        float elapsedTime = 0;
        float runTime = Random.Range(minRunTime, maxRunTime);

        float direction;

        while (elapsedTime < runTime)
        {
            //Calculate direction to run
            direction = Mathf.Sign(transform.position.x - entity.transform.position.x);

            //Check if we looped the map
            if (Vector3.Distance(entity.transform.position, transform.position) > 50f)
            {
                direction *= -1f;
            }

            sr.flipX = direction < 0;

            if(rb.velocity.magnitude < maxRunSpeed)
                rb.AddForce(Vector2.right * runForce * direction);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        direction = Mathf.Sign(transform.position.x - entity.transform.position.x);
        sr.flipX = direction > 0;

        rb.velocity = Vector2.zero;
        running = false;
        animator.SetBool("Running", false);
    }

    IEnumerator Shoot(Transform entity)
    {
        float direction = Mathf.Sign(transform.position.x - entity.transform.position.x);
        sr.flipX = direction > 0;

        yield return new WaitForSeconds(shootDelay);

        //Shoot lance
        PlaySpearSound();

        GameObject spear = Instantiate(spearPrefab, transform.position + (Vector3.up / 2f), Quaternion.identity) as GameObject;
        spear.GetComponent<SpriteRenderer>().flipX = direction > 0;

        Rigidbody2D spearRb = spear.GetComponent<Rigidbody2D>();
        spearRb.AddForce(new Vector2(-direction * Random.Range(minSpearForce, maxSpearForce) * 2f, Random.Range(minSpearForce, maxSpearForce)));

        yield return new WaitForSeconds(rechargeDelay);

        attacking = false;
    }

    private void PlaySpearSound()
    {
        audioSource.clip = spearSounds[Random.Range(0, spearSounds.Length)];
        audioSource.Play();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, runRadius);

        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, attackRadius);
    }
}
