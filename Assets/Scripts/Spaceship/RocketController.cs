using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RocketController : MonoBehaviour {

    [SerializeField] private Transform rocketPartsContainer;
    [SerializeField] private float crashTriggerDelay = 3f;
    private bool blastoff = false;
    private float blastoffTime;

    private Rigidbody2D rb;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            Launch();
    }

    public void Launch()
    {
        blastoff = true;
        blastoffTime = Time.time;

        //Prepare Components
        FixedJoint2D[] joints = GetComponents<FixedJoint2D>();

        List<ThrusterController> thrusters = new List<ThrusterController>();
        List<WingController> wings = new List<WingController>();
        CockpitController finalCockpit = null;

        float enginePower = 0f;
        float noselMultiplier = 1f;

        foreach (FixedJoint2D joint in joints)
        {
            Rigidbody2D rocketPart = joint.connectedBody;
            rocketPart.transform.parent = this.transform;

            LoopableObject loopObj = rocketPart.GetComponent<LoopableObject>();
            if (loopObj != null)
                Destroy(loopObj);

            EngineController engine = rocketPart.GetComponent<EngineController>();
            NoselController nosel = rocketPart.GetComponent<NoselController>();
            ThrusterController thruster = rocketPart.GetComponent<ThrusterController>();
            WingController wing = rocketPart.GetComponent<WingController>();
            CockpitController cockpit = rocketPart.GetComponent<CockpitController>();

            if (engine != null)
            {
                enginePower += engine.power;
            }

            if (nosel != null)
            {
                noselMultiplier *= nosel.multiplier;
            }

            if (thruster != null)
            {
                thrusters.Add(thruster);
            }

            if (wing != null)
            {
                wings.Add(wing);
            }

            if (cockpit != null)
            {
                finalCockpit = cockpit;
            }
        }

        float finalPower = enginePower * noselMultiplier;
        finalPower /= thrusters.Count;
        
        foreach (ThrusterController thruster in thrusters)
            thruster.SetForce(finalPower);

        if (thrusters.Count > 0)
        {
            foreach(WingController wing in wings)
                wing.Activate();
        }

        if(finalCockpit != null) finalCockpit.Enter();

        //Launch
        rb.isKinematic = false;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (blastoff && (Time.time - blastoffTime) > crashTriggerDelay)
        {
            SelfDestruct();
        }
    }

    public void SelfDestruct()
    {
        blastoff = false;

        FixedJoint2D[] joints = GetComponents<FixedJoint2D>();

        foreach (FixedJoint2D joint in joints)
        {
            Rigidbody2D rocketPart = joint.connectedBody;
            rocketPart.transform.parent = rocketPartsContainer;

            rocketPart.gameObject.AddComponent<LoopableObject>();
            
            rocketPart.velocity = Vector2.zero;
            Destroy(joint);

            ThrusterController thruster = rocketPart.GetComponent<ThrusterController>();
            WingController wing = rocketPart.GetComponent<WingController>();
            CockpitController cockpit = rocketPart.GetComponent<CockpitController>();

            if (thruster != null)
            {
                thruster.SetForce(0f);
            }

            if (wing != null)
            {
                wing.Deactivate();
            }

            if (cockpit != null)
            {
                cockpit.Exit();
            }
        }

        rb.isKinematic = true;
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}
