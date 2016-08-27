using UnityEngine;
using System.Collections;

public class DinosaurController : MonoBehaviour {

    [SerializeField] private Rigidbody2D motor;
    [SerializeField] private HingeJoint2D joint;

    [SerializeField] private float motorVelocity = 100f;
    [SerializeField] private float jointVelocity = 100f;

	void Start () 
    {
	
	}
	
	void Update ()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (Input.GetKey(KeyCode.D))
        {
            motor.angularVelocity = -motorVelocity;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            motor.angularVelocity = motorVelocity;
        }
        else
        {
            motor.angularVelocity = 0f;
        }

        JointMotor2D jmotor = joint.motor;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            jmotor.motorSpeed = -jointVelocity;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            jmotor.motorSpeed = jointVelocity;
        }
        else
        {
            jmotor.motorSpeed = 0f;
        }

        joint.motor = jmotor;
    }
}
