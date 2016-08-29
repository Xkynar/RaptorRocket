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
        LoopObjects();

        //Follow player
        if(follow != null) transform.position = follow.position;
	}

    private void LoopObjects()
    {
        foreach (LoopableObject loopableObject in FindObjectsOfType<LoopableObject>())
        {
            //Loop player
            if (loopableObject.transform.position.x > groundExtents.x)
            {
                loopableObject.transform.position = new Vector3(-groundExtents.x, loopableObject.transform.position.y, loopableObject.transform.position.z);
            }

            if (loopableObject.transform.position.x < -groundExtents.x)
            {
                loopableObject.transform.position = new Vector3(groundExtents.x, loopableObject.transform.position.y, loopableObject.transform.position.z);
            }
        }
    }

    public void SetFollow(Transform follow)
    {
        this.follow = follow;
    }

    public Camera GetMainCamera()
    {
        return mainCam;
    }
}
