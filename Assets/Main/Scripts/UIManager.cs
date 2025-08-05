using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public GameObject closeAnim;
    private Scene scene;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        closeAnim.SetActive(false);
    }

    public void LoadSceneBtn(string sceneName)
    {
        closeAnim.SetActive(true);
        if (scene.name == "GameScene")
        {
            GameManager.Instance.PlayChessDownAudio(GameManager.Instance.clips[1]);
        }
        StartCoroutine(SceneLoader(sceneName));
    }

    IEnumerator SceneLoader(string scene)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }
}