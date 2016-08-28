using UnityEngine;
using System.Collections;

public class CockpitController : MonoBehaviour {

    [SerializeField] private GameObject dinosaur;
    [SerializeField] private CameraController cameraCtrl;


    public void Enter()
    {
        dinosaur.SetActive(false);
        cameraCtrl.SetFollow(gameObject.transform);
    }
}
