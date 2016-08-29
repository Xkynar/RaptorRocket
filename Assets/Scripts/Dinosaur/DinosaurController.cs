using UnityEngine;
using System.Collections;

public class DinosaurController : MonoBehaviour {

    [SerializeField] private Rigidbody2D legs;
    [SerializeField] private Rigidbody2D body;

    [SerializeField] private Animator fLegAnim;
    [SerializeField] private Animator bLegAnim;

    [SerializeField] private float motorVelocity = 100f;
    [SerializeField] private float jumpForce = 600f;
    [SerializeField] private float centerOfMassLimit = 5f;

    //Flipping
    [SerializeField] private Transform[] flipParts;

    //Jumping
    [SerializeField] private LayerMask jumpLayermask;
    [SerializeField] private float rayLength = 1f;
    private bool isGrounded = false;

    //Audio
    [SerializeField] private AudioSource footstepAudiosource;
    [SerializeField] private AudioClip[] footstepSounds;

    [SerializeField] private AudioSource jumpAudiosource;
    [SerializeField] private AudioClip[] jumpSounds;

	void Start () 
    {
        //Make center of mass a little higher
        Vector2 centerOfMass = body.centerOfMass;
        centerOfMass.y = 1f;
        body.centerOfMass = centerOfMass;
	}
	
	void Update ()
    {
        UpdateJump();
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (Input.GetKey(KeyCode.D))
        {
            legs.angularVelocity = -motorVelocity;
            fLegAnim.SetBool("Walking", true);
            bLegAnim.SetBool("Walking", true);
            FlipDinosaur(false);
            PlayFootstepSound();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            legs.angularVelocity = motorVelocity;
            fLegAnim.SetBool("Walking", true);
            bLegAnim.SetBool("Walking", true);
            FlipDinosaur(true);
            PlayFootstepSound();
        }
        else
        {
            legs.angularVelocity = 0f;
            fLegAnim.SetBool("Walking", false);
            bLegAnim.SetBool("Walking", false);
        }

        //Update center of mass balancing
        Vector2 centerOfMass = body.centerOfMass;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            centerOfMass.x = -centerOfMassLimit;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            centerOfMass.x = centerOfMassLimit;
        }
        else
        {
            centerOfMass.x = 0f;
        }

        body.centerOfMass = centerOfMass;
    }

    private void UpdateJump()
    {
        RaycastHit2D hit = Physics2D.Raycast(body.transform.position, -body.transform.up, rayLength, jumpLayermask);

        if (hit)
        {
            isGrounded = true;
            Debug.DrawLine(body.transform.position, hit.point, Color.red);
        }
        else
        {
            isGrounded = false;
            Debug.DrawLine(body.transform.position, body.transform.position + (-body.transform.up * rayLength), Color.red);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Vector2 force = Vector2.up * jumpForce;

            foreach(Rigidbody2D bodyPart in gameObject.GetComponentsInChildren<Rigidbody2D>())
                bodyPart.AddForce(force * bodyPart.mass);

            PlayJumpSound();
        }
    }

    private void FlipDinosaur(bool left)
    {
        foreach (Transform part in flipParts)
        {
            Vector3 partScale = part.localScale;

            bool isLeft = partScale.x > 0;

            if (left == isLeft)
                continue;

            partScale.x *= -1f;
            part.localScale = partScale;

            if (part != body.transform)
            {
                Vector3 partPos = part.localPosition;
                partPos.x *= -1f;
                part.localPosition = partPos;
            }

        }
    }

    public GameObject GetBody()
    {
        return body.gameObject;
    }

    //Audio
    private void PlayFootstepSound()
    {
        if (!footstepAudiosource.isPlaying && isGrounded)
        {
            footstepAudiosource.clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
            footstepAudiosource.Play();
        }
    }

    private void PlayJumpSound()
    {
        jumpAudiosource.clip = jumpSounds[Random.Range(0, jumpSounds.Length)];
        jumpAudiosource.Play();
    }

    //Debug
    void OnDrawGizmos()
    {
        //Draw center of mass
        Vector3 centerOfMassPos = body.transform.TransformPoint(body.centerOfMass);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(centerOfMassPos, 0.1f);
    }
}
