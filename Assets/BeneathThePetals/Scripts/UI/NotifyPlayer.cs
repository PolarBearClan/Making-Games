using System;
using UnityEngine;

public class NotifyPlayer : MonoBehaviour
{
    private PlayerController playerController;

    [TextArea]
    [SerializeField] private string notificationText;
    [SerializeField] private int notificationDuration;
    [SerializeField] private bool destroyAfterNotified = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        playerController.ScreenNoteManagerScript.ShowNoteNotification(notificationText, notificationDuration);
        
        if (destroyAfterNotified) Destroy(gameObject);
    }
}
