using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider slider;

    public void LoadLevel(string lvtoload)
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadAsynchronously(lvtoload));
    }
    IEnumerator LoadAsynchronously(string lvtoload)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(lvtoload);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;

            yield return null;
        }
    }
}
