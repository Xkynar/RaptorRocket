using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera leftCam;
    [SerializeField] private Camera rightCam;

    [SerializeField] private SpriteRenderer groundRenderer;
    [SerializeField] private Transform follow;

    private Vector3 groundExtents;

	void Start ()
    {
        groundExtents = groundRenderer.bounds.extents;

        Vector3 camOffset = groundExtents * 2f;
        camOffset.y = 0;
        camOffset.z = 0;

        leftCam.transform.position -= camOffset;
        rightCam.transform.position += camOffset;
	}
	
	void Update ()
    {
        //Loop player
        if (follow.position.x > groundExtents.x)
        {
            follow.position = new Vector3(-groundExtents.x, follow.position.y, follow.position.z);
        }

        if (follow.position.x < -groundExtents.x)
        {
            follow.position = new Vector3(groundExtents.x, follow.position.y, follow.position.z);
        }

        //Follow player
        transform.position = follow.position;
	}

    public void SetFollow(Transform follow)
    {
        this.follow = follow;
    }
}
