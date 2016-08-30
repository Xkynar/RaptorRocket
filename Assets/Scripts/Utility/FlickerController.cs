using UnityEngine;
using System.Collections;

public class FlickerController : MonoBehaviour {

    [SerializeField] private float flickerCount = 5f;
    [SerializeField] private float flickerInterval = 0.4f;

    public void Flicker()
    {
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        SpriteRenderer[] renderers = gameObject.GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < flickerCount; i++)
        {
            SetSpriteAlpha(renderers, 0f);
            yield return new WaitForSeconds(flickerInterval);
            SetSpriteAlpha(renderers, 1f);
            yield return new WaitForSeconds(flickerInterval);
        }
    }

    private void SetSpriteAlpha(SpriteRenderer[] renderers, float alpha)
    {
        foreach (SpriteRenderer renderer in renderers)
        {
            Color color = renderer.color;
            color.a = alpha;
            renderer.color = color;
        }
    }
}
