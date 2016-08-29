using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour {

    private MovieTexture movieTexture;

    void Start()
    {
        movieTexture = ((MovieTexture)GetComponent<RawImage>().material.mainTexture);
        StartCoroutine(DestroyVideo(movieTexture.duration));
        movieTexture.Play();
    }

    IEnumerator DestroyVideo(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Game");
    }
}
