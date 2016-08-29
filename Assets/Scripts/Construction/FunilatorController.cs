using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FunilatorController : MonoBehaviour {

    //Drop
    [SerializeField] private Sprite funilatorSprite;
    [SerializeField] private SmokeController smoke;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private string funilatorLayer;
    private bool isBox = true;

    //Containers
    [SerializeField] private Transform rocketPartsContainer;
    [SerializeField] private Transform poopsContainer;

    //Construction
    private List<MaterialType> materials; //What is put inside

    [SerializeField] private GameObject funilatorBar;
    [SerializeField] private Transform exit;
    [SerializeField] private GameObject poopPrefab;

    [System.Serializable]
    private class Recipe
    {
        public List<MaterialType> materials;
        public List<GameObject> outputs;
    }

    [SerializeField] private List<Recipe> recipes;

    //Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    private bool building = false;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        materials = new List<MaterialType>();

        Drop();
	}
	
	void Update ()
    {
        if (building)
        {
            float rate = audioSource.time / audioSource.clip.length;
            rate *= 1.27f; //to make "pop" sound synchronized

            Vector3 funilatorBarScale = funilatorBar.transform.localScale;
            funilatorBarScale.x = rate;
            funilatorBar.transform.localScale = funilatorBarScale;

            if (rate >= 1f)
            {
                building = false;

                funilatorBarScale.x = 0f;
                funilatorBar.transform.localScale = funilatorBarScale;

                Combine();
            }
        }
	}

    public void Drop()
    {
        //Make box affected by gravity
        rb.isKinematic = false;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Transform into funilator
        if (isBox)
        {
            rb.isKinematic = true;
            smoke.Puff();
            sr.sprite = funilatorSprite;
            gameObject.layer = LayerMask.NameToLayer(funilatorLayer);
            Destroy(boxCollider);

            isBox = false;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        MaterialController matCtrl = coll.gameObject.GetComponent<MaterialController>();

        if (matCtrl != null)
        {
            materials.Add(matCtrl.material);
            Destroy(coll.gameObject);

            audioSource.Play();
            building = true;
        }
    }

    //Construction
    public void Combine()
    {
        if (materials.Count <= 0)
            return;

        List<GameObject> outputs = null;
        
        foreach (Recipe recipe in recipes)
        {
            if (ScrambledEquals(recipe.materials, materials))
            {
                outputs = recipe.outputs;
                break;
            }
        }

        if (outputs == null)
        {
            GameObject poop = Instantiate(poopPrefab, exit.position, Quaternion.identity) as GameObject;
            poop.transform.parent = poopsContainer;
        }
        else
        {
            foreach (GameObject output in outputs)
            {
                GameObject rocketPart = Instantiate(output, exit.position, Quaternion.identity) as GameObject;
                rocketPart.transform.parent = rocketPartsContainer;
            }
        }

        materials.Clear();
    }

    public static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
    {
        var cnt = new Dictionary<T, int>();

        foreach (T s in list1)
        {
            if (cnt.ContainsKey(s))
            {
                cnt[s]++;
            } 
            else
            {
                cnt.Add(s, 1);
            }
        }

        foreach (T s in list2)
        {
            if (cnt.ContainsKey(s))
            {
                cnt[s]--;
            }
            else
            {
                return false;
            }
        }

        return cnt.Values.All(c => c == 0);
    }
}
