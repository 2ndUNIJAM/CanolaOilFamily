using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    private void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    public void OnClickStartButton()
    {
        StartGame();
    }

    public void OnClickQuitButton()
    {
        QuitGame();
    }
}
