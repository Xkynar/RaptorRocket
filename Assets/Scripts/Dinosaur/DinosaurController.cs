using UnityEngine;
using System.Collections;

public class DinosaurController : MonoBehaviour {

    [SerializeField] private Rigidbody2D legs;
    [SerializeField] private Rigidbody2D body;

    [SerializeField] private Animator fLegAnim;
    [SerializeField] private Animator bLegAnim;

    [SerializeField] private float motorVelocity = 100f;
    [SerializeField] private float jumpForce = 3000f;
    private Vector2 centerOfMassIncrement = new Vector2(0.1f, 0f);
    private float centerOfMassLimit = 5f;

    //Flipping
    [SerializeField] private Transform[] flipParts;

	void Start () 
    {
        //Make center of mass a little higher
        Vector2 centerOfMass = body.centerOfMass;
        centerOfMass.y = 1f;
        body.centerOfMass = centerOfMass;
	}
	
	void Update ()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            legs.AddForce(Vector2.up * jumpForce);

        if (Input.GetKey(KeyCode.D))
        {
            legs.angularVelocity = -motorVelocity;
            fLegAnim.SetBool("Walking", true);
            bLegAnim.SetBool("Walking", true);
            FlipDinosaur(false);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            legs.angularVelocity = motorVelocity;
            fLegAnim.SetBool("Walking", true);
            bLegAnim.SetBool("Walking", true);
            FlipDinosaur(true);
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
            centerOfMass -= centerOfMassIncrement;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            centerOfMass += centerOfMassIncrement;
        }

        centerOfMass.x = Mathf.Clamp(centerOfMass.x, -centerOfMassLimit, centerOfMassLimit);

        body.centerOfMass = centerOfMass;
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

    void OnDrawGizmos()
    {
        //Draw center of mass
        Vector3 centerOfMassPos = body.transform.TransformPoint(body.centerOfMass);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(centerOfMassPos, 0.1f);
    }
}
