using System.Collections.Generic;
using UnityEngine;

public class PathingManager : MonoBehaviour
{
    [Tooltip("How many waypoints will be walked through in the INNER circle. Outside circle is  n - 1")]
    [SerializeField] private int changeCirclesAfterPoints = 0;
    
    [Space]
    [SerializeField] private List<Transform> pointsInner;
    [SerializeField] private List<Transform> pointsOuter;
    
    private int currentPoints = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (pointsInner.Count != pointsOuter.Count)
            Debug.LogWarning("PointsInner.Count != pointsOuter.Count. Unexpected behaviour may occur!");
        currentPoints = pointsOuter.Count;
    }

    // Update is called once per frame
    void Update()
    {   
        
    }

    public Transform GetPoint(int index, bool innerCircle)
    {
        return innerCircle ? pointsInner[index % pointsInner.Count] : pointsOuter[index % pointsOuter.Count];
    }
    
    public int ChangeCirclesAfterPoints => changeCirclesAfterPoints;
}
