using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMan : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private Vector3 length;
    private Vector3 _initialPos;
    private float _anim = 0f;
    private int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        _initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _anim += direction * speedMultiplier * Time.deltaTime;
        if (_anim < 0f)
        {
            _anim = 0f;
            direction = 1;
        }
        else if (_anim > 1f)
        {
            _anim = 1f;
            direction = -1;
        }

        transform.position = _initialPos + length * _anim;
    }
}