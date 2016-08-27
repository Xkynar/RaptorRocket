using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PickableController : MonoBehaviour {

    [SerializeField] private string pickedLayer = "Picked";
    private int originalLayer;
    private Rigidbody2D rb;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalLayer = gameObject.layer;
        originalScale = transform.localScale;
    }

    public void Pick(HandController hand)
    {
        rb.isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer(pickedLayer);
        transform.parent = hand.transform;
        transform.localScale = originalScale;
    }

    public void Drop()
    {
        rb.isKinematic = false;
        gameObject.layer = originalLayer;
        transform.parent = null;
        transform.localScale = originalScale;
    }
}
