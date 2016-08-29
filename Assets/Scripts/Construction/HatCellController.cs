using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HatCellController : MonoBehaviour {

    [SerializeField] private float radius = 10f;
    [SerializeField] private LayerMask layerMask;

    public bool inRange = false;

	void FixedUpdate ()
    {
        if (Physics2D.OverlapCircle(transform.position, radius, layerMask) != null)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }
	}

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
        Gizmos.DrawSphere(transform.position, radius);
    }
}
