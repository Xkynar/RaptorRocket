using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [SerializeField] private Camera mainCam, leftCam, rightCam;
    [SerializeField] private SpriteRenderer groundRenderer;
    [SerializeField] private Transform player;

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
        if (player.position.x > groundExtents.x)
        {
            player.position = new Vector3(-groundExtents.x, player.position.y, player.position.z);
        }

        if (player.position.x < -groundExtents.x)
        {
            player.position = new Vector3(groundExtents.x, player.position.y, player.position.z);
        }

        //Follow player
        transform.position = player.position;
	}
}
