using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _credit;

    private void StartGame()
    {
        SceneManager.LoadScene("StoryScene");
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _credit.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _credit.SetActive(false);
        }
    }
}
