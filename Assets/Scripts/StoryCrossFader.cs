using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryCrossFader: MonoBehaviour
{
    private float timer = 0f;
    [SerializeField] private Image _currentImage;
    [SerializeField] private Image _nextImage;
    [SerializeField] private Sprite[] images;
    private int _currentIndex = 0;
    private float _duration = 0.5f;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1f) timer = 1f;
        
    }

    public void StartNextAnim()
    {
        if (_currentIndex != images.Length - 1)
        {
            _currentImage.sprite = images[_currentIndex++];
            _nextImage.sprite = images[_currentIndex];
            _currentImage.CrossFadeAlpha(1,0f,true);
            _nextImage.CrossFadeAlpha(0, 0f, true);
            _currentImage.CrossFadeAlpha(0, _duration, false);
            _nextImage.CrossFadeAlpha(1, _duration, false);
        }
        else
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}