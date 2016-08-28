using UnityEngine;
using System.Collections;

public class WingController : MonoBehaviour {

    private Rigidbody2D rb;
    [SerializeField] private float force = 100f;
    [SerializeField] private bool rightWing;

    private bool activated = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (activated)
        {
            float angle = rb.transform.rotation.eulerAngles.z;
            if (angle > 180)
                angle =  -(360 - angle);

            rb.AddTorque((rightWing ? 1f : -1f) * angle * force * Time.deltaTime);
        }
    }

    public void Activate()
    {
        activated = true;
    }
}
