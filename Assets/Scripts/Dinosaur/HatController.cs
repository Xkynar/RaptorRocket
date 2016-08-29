using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HatController : MonoBehaviour {

    [SerializeField] private Camera cam;
    [SerializeField] private SpriteRenderer laserSprite;
    [SerializeField] private float telekinesisForce = 3f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Image hatLaserIcon;

    private Rigidbody2D picked;
    private bool hasEnergy = false;

    //Components
    private AudioSource audioSource;

    void Start ()
    {
        audioSource = GetComponent<AudioSource>();    
	}
	
    void Update ()
    {
        UpdateEnergy();

        if (hasEnergy)
        {
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, float.MaxValue, layerMask);

                if (hit.collider != null)
                {
                    PickableController pickable = hit.collider.gameObject.GetComponent<PickableController>();

                    if (pickable != null)
                    {
                        picked = pickable.GetComponent<Rigidbody2D>();
                        audioSource.Play();
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                Drop();
            }


            if (picked != null)
            {
                mouseWorldPos.z = 0;

                Vector2 direction = mouseWorldPos - picked.transform.position;
                picked.velocity = direction * telekinesisForce;

                laserSprite.enabled = true;
                PositionLaser(picked.transform.position, transform.position);
            }
        }
        else
        {
            if (picked != null)
                Drop();
        }
	}

    private void Drop()
    {
        picked = null;
        audioSource.Stop();
        laserSprite.enabled = false;
    }

    private void PositionLaser(Vector3 p1, Vector3 p2)
    {
        //Place sprite in center position
        Vector3 centerPos = (p1 + p2) / 2f;
        laserSprite.transform.position = centerPos;

        //Shove it to the correct right location
        Vector3 direction = (p2 - p1).normalized;
        laserSprite.transform.right = direction;

        //Scale it properly so it reaches the other point
        Vector3 scale = Vector3.one;
        scale.x = Vector3.Distance(p1, p2) / laserSprite.sprite.bounds.size.x / transform.localScale.x;
        laserSprite.transform.localScale = scale;
    }

    private void UpdateEnergy()
    {
        HatCellController[] hatCells = FindObjectsOfType<HatCellController>();

        hasEnergy = false;
        foreach (HatCellController hatCell in hatCells)
        {
            hasEnergy |= hatCell.inRange;
        }

        Color color = hatLaserIcon.color;
        color.a = hasEnergy ? 1f : 0.2f;
        hatLaserIcon.color = color;
    }
}
