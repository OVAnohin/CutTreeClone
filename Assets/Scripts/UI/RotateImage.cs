using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RotateImage : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;

    private RectTransform _rectTransform;
    private float _deltaRotate = 0;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_deltaRotate >= 1 || _deltaRotate <= -1)
            _rotateSpeed *= -1;

        _deltaRotate += _rotateSpeed;
        _rectTransform.localScale = new Vector3(_deltaRotate, 1, 1);
    }
}
