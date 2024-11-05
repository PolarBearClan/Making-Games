using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;


public class Jumpscare_1 : MonoBehaviour, IScareable
{
    [SerializeField]
    private GameObject _jumpscareObject;

    [SerializeField]
    private float _duration;

    private GameObject _player;
    private FirstPersonController _playerController;
    private Camera _playerCamera;

    private float _tweenDuration = .1f;

    private bool _triggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<FirstPersonController>();
        _playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (_triggered && _duration > 0)
        {
            _duration -= Time.deltaTime;
        }
        if (_duration <= 0)
        {
            _playerController.EnableInput();
            Destroy(this);
            Destroy(_jumpscareObject);
        }
    }

    public void Scare()
    {
        _playerController.DisableInput();
        _playerController.playerCamera.transform.DOLookAt(_jumpscareObject.transform.position, _tweenDuration);
        _playerController.transform.DOLookAt(_jumpscareObject.transform.position, _tweenDuration);
        Debug.Log("Jumpscared");
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject == _player) && !_triggered)
        {
            _triggered = true;
            Debug.Log("Jumpscare triggered");
            Scare();
        }
    }
}
