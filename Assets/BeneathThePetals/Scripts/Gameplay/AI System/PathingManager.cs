using System.Collections.Generic;
using UnityEngine;

public class PathingManager : MonoBehaviour
{
    public int changeCirclesAfterPoints = 0;
    
    public List<Transform> pointsInner;
    public List<Transform> pointsOuter;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetPoint(int index, bool innerCircle)
    {
        return innerCircle ? pointsInner[index % pointsInner.Count] : pointsOuter[index % pointsOuter.Count];
    }
}
