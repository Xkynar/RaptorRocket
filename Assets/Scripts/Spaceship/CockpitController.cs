using UnityEngine;
using System.Collections;

public class CockpitController : MonoBehaviour {

    [SerializeField] private DinosaurController dinosaur;
    [SerializeField] private CameraController cameraCtrl;
    [SerializeField] private SmokeController smoke;


    public void Enter()
    {
        dinosaur.gameObject.SetActive(false);
        cameraCtrl.SetFollow(gameObject.transform);
    }

    public void Exit()
    {
        dinosaur.gameObject.SetActive(true);
        dinosaur.GetBody().transform.position = transform.position + Vector3.up;
        cameraCtrl.SetFollow(dinosaur.GetBody().transform);

        smoke.Puff();
    }
}
