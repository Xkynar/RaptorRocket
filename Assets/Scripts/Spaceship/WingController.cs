using UnityEngine;
using System.Collections;

public class WingController : RocketPart {

    private Rigidbody2D rb;
    [SerializeField] private float force = 100f;
    [SerializeField] private bool rightWing;
    [SerializeField] private GameObject thrusterEffect;

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
        thrusterEffect.SetActive(true);
    }

    public void Deactivate()
    {
        activated = false;
        thrusterEffect.SetActive(false);
    }
}
