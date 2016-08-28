using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour {

    [SerializeField] private Camera cam;
    private SpriteRenderer sr;

	void Start ()
    {
        sr = GetComponent<SpriteRenderer>();

        float backgroundWidth = sr.bounds.extents.x * 2f;
        float cameraWidth = cam.orthographicSize * 2.0f * Screen.width / Screen.height;
        float rate = cameraWidth / backgroundWidth;

        transform.localScale *= rate;
	}
	
	void LateUpdate ()
    {
        Vector3 pos = transform.position;
        pos.x = cam.transform.position.x;
        transform.position = pos;
	}
}
