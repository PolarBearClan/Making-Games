using System;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform playerCamera;

    void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GetCamera();
    }

    void LateUpdate()
    {
        if (playerCamera != null)
            transform.LookAt(transform.position + playerCamera.forward);
        else
            Debug.LogWarning("Player Camera is not assigned to the Billboard script!");
    }
}
