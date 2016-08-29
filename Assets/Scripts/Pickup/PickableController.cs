using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PickableController : MonoBehaviour {
    
    [SerializeField] private string pickedLayer = "Picked";
    private int originalLayer;
    private Rigidbody2D rb;

    private Vector3 originalScale;
    private Transform originalParent;

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
        originalParent = transform.parent;
        transform.parent = hand.transform;
        transform.localScale = originalScale;

        LoopableObject loopObj = GetComponent<LoopableObject>();
        if (loopObj != null)
            Destroy(loopObj);
    }

    public void Drop()
    {
        rb.isKinematic = false;
        gameObject.layer = originalLayer;
        transform.parent = originalParent;
        transform.localScale = originalScale;

        gameObject.AddComponent<LoopableObject>();
    }
}
