using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RocketController : MonoBehaviour {

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
        //Prepare Components
        FixedJoint2D[] joints = GetComponents<FixedJoint2D>();

        List<ThrusterController> thrusters = new List<ThrusterController>();
        List<WingController> wings = new List<WingController>();
        CockpitController finalCockpit = null;

        float enginePower = 0f;
        float noselMultiplier = 1f;

        foreach (FixedJoint2D joint in joints)
        {
            EngineController engine = joint.connectedBody.GetComponent<EngineController>();
            NoselController nosel = joint.connectedBody.GetComponent<NoselController>();
            ThrusterController thruster = joint.connectedBody.GetComponent<ThrusterController>();
            WingController wing = joint.connectedBody.GetComponent<WingController>();
            CockpitController cockpit = joint.connectedBody.GetComponent<CockpitController>();

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
}
