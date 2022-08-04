using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FrozenBackgroundController : MonoBehaviour
{
    private static readonly int xProperty = Shader.PropertyToID("_x");
    private static readonly int yProperty = Shader.PropertyToID("_y");
    private static readonly int radiusProperty = Shader.PropertyToID("_radius");
    
    [SerializeField]
    private RawImage rawImage;

    public bool IsFreezing => isFreezing;

    public float TransitionDuration = 3f;
    public float MinRadius = 0.1f;
    public float MaxRadius = 1f;

    private Material material;
    private float startTime;
    private bool transitionInProgress = false;
    private bool isFreezing = true;

    public void WarmUp(Vector2 point)
    {
        StartTransition(point, false);
    }

    public void Freeze()
    {
        Freeze(new Vector2(0.5f, 0.5f));
    }

    private void Freeze(Vector2 point)
    {
        StartTransition(point, true);
    }

    private void Start()
    {
        if (!rawImage)
            rawImage = GetComponent<RawImage>();

        material = rawImage.material;
        
        ResetMaterial();
    }

    private void OnDestroy()
    {
        ResetMaterial();
    }

    private void Update()
    {
        if (transitionInProgress)
            UpdateTransition();
    }

    private void ResetMaterial()
    {
        material.SetFloat(radiusProperty, 0);
    }

    private void StartTransition(Vector2 point, bool freeze)
    {
        material.SetFloat(xProperty, point.x);
        material.SetFloat(yProperty, point.y);
        transitionInProgress = true;
        startTime = Time.time;

        isFreezing = freeze;
    }

    private void UpdateTransition()
    {
        var progress = (Time.time - startTime) / TransitionDuration;

        if (progress > 1) 
            transitionInProgress = false;
        
        if (isFreezing)
            progress = 1 - progress;

        var radius = MinRadius + (MaxRadius - MinRadius) * progress; 
        
        material.SetFloat(radiusProperty, radius);
    }
}
