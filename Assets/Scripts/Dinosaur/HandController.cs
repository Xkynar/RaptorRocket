using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour {

    private PickableController picked;

	void Start ()
    {
	
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (picked == null)
            {
                Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 0.5f);

                foreach(Collider2D coll in colls)
                {
                    PickableController pickable = coll.gameObject.GetComponent<PickableController>();

                    if (pickable != null)
                    {
                        pickable.Pick(this);
                        picked = pickable;
                        break;
                    }   
                }
            }
            else
            {
                picked.Drop();
                picked = null;
            }
        }
	}

    public void Drop()
    {
        if(picked != null) picked.Drop();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
