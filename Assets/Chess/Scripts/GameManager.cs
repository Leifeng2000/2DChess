using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    public GameObject gameCanvas;
    public AudioSource chessDownAudio;
    public Animator uiCanvasAnim;
    public Text gameOverText;

    public AudioClip[] clips;

    public static bool isGameOver;

    public static string gameOverState;

    private void Awake()
    {
        Instance = this;
    }

    void Start () {
        PlayChessDownAudio(clips[0]);
        isGameOver = false;
        gameCanvas.SetActive(false);
        Invoke("EnableGameCanvas", 0.5f);
    }
	
	void Update () {

        if (isGameOver)
        {
            isGameOver = false;
            uiCanvasAnim.SetTrigger("GameOver");

            switch (gameOverState)
            {
                case "BlackMate":
                    gameOverText.text = "You won! Black chess lost!";
                    break;
                case "WhiteMate":
                    gameOverText.text = "You lose! Black chess wins!";
                    break;
                case "StaleMate":
                    gameOverText.text = "Game over! Stalemate!";
                    break;
            }
        }
	}

    private void EnableGameCanvas()
    {
        gameCanvas.SetActive(true);
        gameCanvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        //Screen.SetResolution(1080, 1920, Screen.fullScreen);
        gameCanvas.GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
    }

    public void PlayChessDownAudio(AudioClip clip)
    {
        chessDownAudio.PlayOneShot(clip);
    }
}
