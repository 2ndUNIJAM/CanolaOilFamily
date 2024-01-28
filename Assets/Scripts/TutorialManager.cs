using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorials;
    private int _idx = 0;

    public void MoveToNext()
    {
        if (_idx != tutorials.Length - 1)
        {
            tutorials[_idx++].SetActive(false);
            tutorials[_idx].SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
