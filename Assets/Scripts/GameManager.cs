using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    [SerializeField] private CameraController camCtrl;
    [SerializeField] private SpriteRenderer background;

    private Camera mainCam;

	// Use this for initialization
	void Start ()
    {
        mainCam = camCtrl.GetMainCamera();
	}
	
	void Update ()
    {
        float backgroundTop = background.bounds.center.y + background.bounds.extents.y;
        float camTop = mainCam.transform.position.y + mainCam.orthographicSize;

        if (camTop >= backgroundTop)
        {
            camCtrl.SetFollow(null);

            Vector3 camPos = camCtrl.transform.position;
            camPos.y = backgroundTop - mainCam.orthographicSize;
            camCtrl.transform.position = camPos;

            Win();
        }
	}

    public void Win()
    {
    }
}
