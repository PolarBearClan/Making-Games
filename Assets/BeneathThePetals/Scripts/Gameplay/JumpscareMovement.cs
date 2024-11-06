using System.Runtime.CompilerServices;
using System.Timers;
using UnityEngine;
using DG.Tweening;

public class JumpscareMovement : Jumpscare
{
    [SerializeField]
    private GameObject _destination;

    [SerializeField]
    private float _lerpDuration;

    private float _timeElapsed;
    private bool _triggered = false;
    private Vector3 _startPosition;

    void Start()
    {
        gameObject.SetActive(false);
        _startPosition = transform.position;
    }

    void Update()
    {
        if (_triggered && _timeElapsed < _lerpDuration)
        {
            transform.position = Vector3.Lerp(_startPosition, _destination.transform.position, _timeElapsed / _lerpDuration);
            _timeElapsed += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void Scare()
    {
        gameObject.SetActive(true);
        _triggered = true;
    }
}
