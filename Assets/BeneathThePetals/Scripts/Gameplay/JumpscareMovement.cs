using System.Runtime.CompilerServices;
using System.Timers;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class JumpscareMovement : Jumpscare
{
    [SerializeField]
    private GameObject _destination;

    [SerializeField]
    private float _lerpDuration;

    [SerializeField]
    private float startWalking = 2f;

    [SerializeField]
    private float rotationDuration = 0.5f;

    private float _timeElapsed;
    private bool _triggered = false;
    private Vector3 _startPosition;
    private bool isCoroutine = false;

    private Animator _anim;

    void Start()
    {
        gameObject.SetActive(false);
        _startPosition = transform.position;
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(isCoroutine)
        {
            transform.position = Vector3.Lerp(_startPosition, _destination.transform.position, Mathf.Clamp01(_timeElapsed / _lerpDuration));
            _timeElapsed += Time.deltaTime;
        }

        /*
        if (_triggered && _timeElapsed < _lerpDuration)
        {
            _anim.SetBool("isWalking", true);

            transform.position = Vector3.Lerp(_startPosition, _destination.transform.position, _timeElapsed / _lerpDuration);
            _timeElapsed += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
        */
    }

    public override void Scare()
    {
        gameObject.SetActive(true);
        _triggered = true;
        StartCoroutine(StartSequence());
    }

    private IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(startWalking);
        RotateTowardsDestination();
        _anim.SetBool("isWalking", true);
        isCoroutine = true;
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private void RotateTowardsDestination()
    {
        Vector3 direction = (_destination.transform.position - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.DORotateQuaternion(targetRotation, rotationDuration);
    }
}
