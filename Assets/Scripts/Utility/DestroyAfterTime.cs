using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

    [SerializeField] private float time;

	void Start ()
    {
        Destroy(gameObject, time);
	}
}
