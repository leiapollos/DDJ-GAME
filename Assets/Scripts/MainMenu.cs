using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public GameObject loadingScreen;
    public Slider slider;
    public Text LoadingTextBackground;
    public Text LoadingText;


    public void LoadLevel()
    {
        StartCoroutine(LoadAsynchronously());

    }

    IEnumerator LoadAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Level1");

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;
            LoadingText.text = ((int)(progress * 100)).ToString() + "%";
            LoadingTextBackground.text = ((int)(progress * 100)).ToString() + "%";

            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
  
}
