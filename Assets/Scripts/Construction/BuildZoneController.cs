using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildZoneController : MonoBehaviour {

    [SerializeField] private Rigidbody2D rocketFrame;
    [SerializeField] private Camera cam;

    private List<Rigidbody2D> inside;

	void Start ()
    {
        inside = new List<Rigidbody2D>();
	}
	
	void Update ()
    {
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPos, Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    RocketPart rocketPart = hit.collider.gameObject.GetComponent<RocketPart>();

                    if (rocketPart != null)
                    {
                        Rigidbody2D rocketPartRb = rocketPart.GetComponent<Rigidbody2D>();

                        if (inside.Contains(rocketPartRb))
                        {
                            HandleRocketPart(rocketPartRb);
                            break;
                        }
                    }
                }    
            }
        }
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        Rigidbody2D rb = coll.GetComponent<Rigidbody2D>();

        if (rb != null && !inside.Contains(rb))
        {
            inside.Add(rb);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        Rigidbody2D rb = coll.GetComponent<Rigidbody2D>();

        if (rb != null && inside.Contains(rb))
        {
            inside.Remove(rb);
        }
    }

    private void HandleRocketPart(Rigidbody2D rocketPart)
    {
        FixedJoint2D[] joints = rocketFrame.GetComponents<FixedJoint2D>();

        foreach (FixedJoint2D joint in joints)
        {
            if (joint.connectedBody == rocketPart)
            {
                DisconnectRocketPart(rocketPart, joint);
                return;
            }
        }

        ConnectRocketPart(rocketPart);
    }

    //Connecting
    private void ConnectRocketPart(Rigidbody2D rocketPart)
    {
        FixedJoint2D joint = rocketFrame.gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = rocketPart;

        joint.anchor = rocketFrame.gameObject.transform.InverseTransformPoint(rocketPart.transform.position);
    }

    private void DisconnectRocketPart(Rigidbody2D rocketPart, FixedJoint2D joint)
    {
        Destroy(joint);
    }
}
