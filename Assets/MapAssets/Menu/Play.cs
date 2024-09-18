using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Play : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public void ChoiMoi()
    {
        SceneManager.LoadScene(1);
    }
    public void Thoat()
    {
        Application.Quit();
    }
}
