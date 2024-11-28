using System.Runtime.CompilerServices;
using System.Timers;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System.Collections;

public class JumpscareSpawn : Jumpscare
{
    [SerializeField]
    private float _distanceFromPlayer;

    private NPCBaseController _dialogue;
    private GameObject _player;
    private FirstPersonController _playerController;

    [HideInInspector]
    public bool _isPlaced = false;
    private bool _triggered = false;


    void Start()
    {
        gameObject.SetActive(false);
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<FirstPersonController>();
        _dialogue = gameObject.GetComponent<NPCBaseController>();
    }

    void Update()
    {

        if (_triggered && !_isPlaced)
        {
            gameObject.transform.position = _player.transform.position - _player.transform.forward * _distanceFromPlayer;

            // Raycast down from the object's position
            Ray ray = new Ray(gameObject.transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Calculate the required position to touch the ground
                float colliderHeight = gameObject.GetComponent<Collider>().bounds.extents.y;
                Debug.Log(hit.point);

                gameObject.transform.position = hit.point;
                Debug.Log(transform.position.y);
            }

            Vector3 directionToTaget = _player.transform.position - gameObject.transform.position;
            directionToTaget.y = 0;
            gameObject.transform.rotation = Quaternion.LookRotation(directionToTaget);

            //_dialogue.Interact();
            _isPlaced = true;
        }
    }

    public override void Scare()
    {
        gameObject.SetActive(true);
        _triggered = true;
    }
}