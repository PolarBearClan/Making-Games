using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PulsingLight : MonoBehaviour
{
    private RectTransform rectTransform;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        PulseUp();
    }

    private void PulseUp()
    {
        // Example of pulsing
        rectTransform.DOScale(Vector3.one * 2, 0.5f).OnComplete(() => { Invoke(nameof(PulseDown), 0.5f); });
    }

    private void PulseDown()
    {
        // Example of pulsing
        rectTransform.DOScale(Vector3.one, 0.5f).OnComplete(() => { Invoke(nameof(PulseUp), 0.5f); });
    }

}
