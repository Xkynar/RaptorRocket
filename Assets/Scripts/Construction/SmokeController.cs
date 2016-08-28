using UnityEngine;
using System.Collections;

public class SmokeController : MonoBehaviour {

    [SerializeField] private float lerpRate = 0.05f;

    private SpriteRenderer sr;

	void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
	}
	
    public void Puff()
    {
        StartCoroutine(PuffRoutine());
    }

    IEnumerator PuffRoutine()
    {
        Color color = sr.color;
        float startAlpha = 1f;
        float targetAlpha = 0f;

        //Make smoke visible
        color.a = startAlpha;
        sr.color = color;

        //Fade smoke
        while (color.a > 0.01f)
        {
            color.a = Mathf.MoveTowards(color.a, targetAlpha, lerpRate);
            sr.color = color;

            yield return null;
        }

        color.a = targetAlpha;
        sr.color = color;
    }
}
