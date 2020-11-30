﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScissorsMover : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _speed;
    [SerializeField] private SpriteRenderer[] _spriteRendererScissors;
    [SerializeField] private Color _touchColor;
    [SerializeField] private Color _untouchColor;

    private Vector3 _targetPosition;
    private Vector3 _startPosition;

    private void Start()
    {
        _targetPosition = _startPosition = transform.position;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            for (var i = 0; i < Input.touchCount; ++i)
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                    if (Input.GetTouch(i).tapCount == 2)
                        Flip();
        }

        if (_targetPosition != transform.position)
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);
    }

    public void MoveToStartPosition()
    {
        transform.position = _targetPosition = _startPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _targetPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        _targetPosition = new Vector3(_targetPosition.x, _targetPosition.y, transform.position.z);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _targetPosition = transform.position;
        ChangeScissorsColor(_untouchColor);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ChangeScissorsColor(_touchColor);
    }

    private void ChangeScissorsColor(Color color)
    {
        foreach (var item in _spriteRendererScissors)
            item.color = color;
    }

    public void Flip()
    {
        float angle = transform.localRotation.eulerAngles.z * -1;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
