using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    private void MoveScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    public void OnClickMoveSceneButton()
    {
        MoveScene();
    }
}
