using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetector : MonoBehaviour, IPointerClickHandler
{
    public Action<Vector2> OnClick;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = transform as RectTransform;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var position = (Vector2)transform.InverseTransformPoint(eventData.position);
        position += rectTransform.rect.size * rectTransform.pivot;

        var uvCoordinates = position / rectTransform.rect.size;
        
        OnClick?.Invoke(uvCoordinates);
    }
}
