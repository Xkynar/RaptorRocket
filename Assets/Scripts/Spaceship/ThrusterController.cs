using UnityEngine;
using System.Collections;

public class ThrusterController  : MonoBehaviour {

    [SerializeField] private GameObject thrusterEffect;

    private Rigidbody2D rb;
    private float engineForce = 0;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
	}
	
    void Update () 
    {
        if (engineForce > 0)
        {
            rb.AddForce(transform.up * engineForce * Time.deltaTime);
        }
	}

    public void SetForce(float engineForce)
    {
        this.engineForce = engineForce;
        thrusterEffect.SetActive(engineForce > 0);
    }
}
