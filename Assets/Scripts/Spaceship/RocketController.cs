using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RocketController : MonoBehaviour {

    [SerializeField] private Transform rocketPartsContainer;
    [SerializeField] private float crashTriggerDelay = 3f;
    [SerializeField] private Transform dinosaur;
    [SerializeField] private float launchDistance = 10f;
    private bool blastoff = false;
    private float blastoffTime;

    [SerializeField] private Image launchImage;

    //Audio
    [SerializeField] private AudioSource boostSound;
    [SerializeField] private AudioSource fireSound;
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private AudioClip fireLoopClip;

    private Rigidbody2D rb;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
        if (blastoff)
        {           
            if (!fireSound.isPlaying)
            {
                fireSound.clip = fireLoopClip;
                fireSound.loop = true;
                fireSound.Play();
            }    
                
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SelfDestruct();
            }

            return;
        }

        Color color = launchImage.color;

        if (Vector3.Distance(dinosaur.position, transform.position) < launchDistance)
        {
            color.a = 1f;

            if (Input.GetKeyDown(KeyCode.L))
                Launch();
        }
        else
        {
            color.a = 0.2f;
        }

        launchImage.color = color;
    }

    public void Launch()
    {
        //Check if we have at least 1 joint
        FixedJoint2D[] joints = GetComponents<FixedJoint2D>();
        if (joints.Length <= 0)
            return;

        PlayLaunchSound();

        blastoff = true;
        blastoffTime = Time.time;

        //Prepare Components
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

        StopLaunchSound();

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

    private void PlayLaunchSound()
    {
        boostSound.Play();
        fireSound.clip = fireClip;
        fireSound.loop = false;
        fireSound.Play();
    }

    private void StopLaunchSound()
    {
        fireSound.Stop();
    }
}
