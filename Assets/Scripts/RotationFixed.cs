using UnityEngine;
using System.Collections;

public class RotationFixed : MonoBehaviour {
    
	void LateUpdate ()
    {
        transform.rotation = Quaternion.identity;    
	}   
}
