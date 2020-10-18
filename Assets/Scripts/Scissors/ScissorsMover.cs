using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class ScissorsMover : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _speed;

    private SpriteRenderer _spriteRenderer;
    private Color _untouchColor;
    private Color _touchColor = Color.red;
    private Vector3 _targetPosition;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _untouchColor = _spriteRenderer.color;
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (_targetPosition != transform.position)
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _targetPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        _targetPosition = new Vector3(_targetPosition.x, _targetPosition.y, transform.position.z);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _targetPosition = transform.position;
        _spriteRenderer.color = _untouchColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _spriteRenderer.color = _touchColor;
    }
}
