using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaterialManager : MonoBehaviour {

    [SerializeField] private Transform materialContainer;
    [SerializeField] private HatController hatCtrl;
    private Dictionary<GameObject, Vector3> matSpawns; //associated a material with its spawn position

	void Start ()
    {
        matSpawns = new Dictionary<GameObject, Vector3>();

        foreach (Transform mat in materialContainer)
        {
            matSpawns.Add(mat.gameObject, mat.position);
        }
	}

    public void ResetMaterial(GameObject mat)
    {
        if (hatCtrl.IsPicking(mat))
            hatCtrl.Drop();

        if (matSpawns.ContainsKey(mat))
        {
            mat.transform.position = matSpawns[mat];
            mat.transform.rotation = Quaternion.identity;
            Rigidbody2D rb = mat.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        else
        {
            Destroy(mat);
        }
    }
}
