using System;
using UnityEngine;

public class GizmoSphere : MonoBehaviour
{
    public float radius = 0.5f;
    public Color color = Color.blue;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
