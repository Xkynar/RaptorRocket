using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour {

    [SerializeField] private CameraController camCtrl;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private GameObject winImage;
    [SerializeField] private Text winTimeText;

    [SerializeField] private float winDelay = 1f;

    //Start Dialog
    [SerializeField] private GameObject dialog;
    [SerializeField] private Sprite[] dialogSprites;
    [SerializeField] private Image dialogText;
    private int dialogIndex = 0;

    [SerializeField] private FunilatorController funilator;

    private float gameStartTime;
    private bool gameOver = false;

    private Camera mainCam;

	// Use this for initialization
	void Start ()
    {
        mainCam = camCtrl.GetMainCamera();

        gameStartTime = Time.time;
	}
	
	void Update ()
    {
        if (gameOver)
            return;

        //Dialog
        if (dialog.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            if (dialogIndex == 1) //Drop funilator
            {
                funilator.Drop();
            }

            if (dialogIndex >= dialogSprites.Length) //Disable dialog
            {
                dialog.SetActive(false);
                
            }
            else //Show next
            {
                dialogText.sprite = dialogSprites[dialogIndex];
                dialogIndex++;
            }
        }

        //Win condition
        float backgroundTop = background.bounds.center.y + background.bounds.extents.y;
        float camTop = mainCam.transform.position.y + mainCam.orthographicSize;

        if (camTop >= backgroundTop)
        {
            camCtrl.SetFollow(null);

            Vector3 camPos = camCtrl.transform.position;
            camPos.y = backgroundTop - mainCam.orthographicSize;
            camCtrl.transform.position = camPos;

            gameOver = true;

            StartCoroutine(Win());
        }
	}

    IEnumerator Win()
    {
        float gameEndTime = Time.time;
        float gameTime = gameEndTime - gameStartTime;

        TimeSpan t = TimeSpan.FromSeconds( gameTime );

        string timeText = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", 
            t.Hours, 
            t.Minutes, 
            t.Seconds, 
            t.Milliseconds);

        yield return new WaitForSeconds(winDelay);

        winImage.SetActive(true);
        winTimeText.text = timeText;

        yield return new WaitForSeconds(winDelay * 5f);

        SceneManager.LoadScene("Cutscene");
    }
}
