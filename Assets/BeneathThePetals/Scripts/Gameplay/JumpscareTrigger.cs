using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using FMOD.Studio;
using FMODUnity;


public class JumpscareTrigger : MonoBehaviour
{
    [SerializeField]
    private Jumpscare _jumpscareObject;

    [SerializeField]
    private Transform _lookAtObject;

    [SerializeField]
    private float _duration;

    [SerializeField]
    private float _tweenDuration = .1f;

    private GameObject _player;
    private FirstPersonController _playerController;
    private Jumpscare _jumpscare;
    public EventReference jumpscareSound;

    private bool _triggered = false;
    private float _timeElapsed = 0;


    void Start()
    {
        if(GetComponent<MeshRenderer>() != null)
            GetComponent<MeshRenderer>().enabled = false;
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<FirstPersonController>();
        _jumpscare = _jumpscareObject.GetComponent<Jumpscare>();
    }

    void Update()
    {
        if (_triggered)
        {
            if (_timeElapsed == 0)
            {
                if (_jumpscare.GetType() != typeof(JumpscareSpawn))
                {
                    _playerController.playerCamera.transform.DOLookAt(_lookAtObject.transform.position, _tweenDuration);
                    _playerController.transform.DOLookAt(_lookAtObject.transform.position, _tweenDuration);
                }
            }

            if (_timeElapsed >= _tweenDuration)
            {
                if (_jumpscare.GetType() != typeof(JumpscareSpawn))
                    _playerController.playerCamera.transform.DOLookAt(_lookAtObject.transform.position, 0);
            }

            if (_timeElapsed > _duration)
            {
                if (_jumpscare.GetType() != typeof(JumpscareSpawn) &&
                    _jumpscare.GetType() != typeof(UndergroundJumpscare))
                {
                    _playerController.EnableInput();
                    _player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                }

                Destroy(gameObject);
            }
            _timeElapsed += Time.deltaTime;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject == _player) && !_triggered)
        {
            onJumpscareSound();
            _playerController.isWalking = false;
            _triggered = true;

            _playerController.DisableInput();

            _jumpscare.Scare();

            Cursor.visible = false;

            Debug.Log("Jumpscare!");

        }
    }

    private void onJumpscareSound()
    {
        EventInstance  soundOnInteract = RuntimeManager.CreateInstance(jumpscareSound);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    }
}
