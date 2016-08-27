using UnityEngine;
using System.Collections;

public class BlinkController : MonoBehaviour {

    private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        sr.color = new Color(Random.value, Random.value, Random.value);
	}
}
