using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class YouSpinMeRound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform back;
    private int _direction = -1;
    private float _animX = 0f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var newX = _animX + _direction * Time.deltaTime * 10f;
        _animX = newX < 0f ? 0f : newX > 1f ? 1f : newX;
        back.localRotation = Quaternion.Euler(0f, 0f, +15 + 15 * Anim(_animX));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _direction = 1;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _direction = -1;
    }

    private float Anim(float x) => (Mathf.Cos((float)Math.PI * x) - 1) / 2;
}