using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using System.Collections;
using UnityEngine;

public class JumpscareTriggerSpawn : MonoBehaviour
{
    [Header("Jumpscare Variables")]
    [SerializeField] private Jumpscare _jumpscareObject;
    [SerializeField] private float _durationUntilTalk;
    [SerializeField] private float _tweenDuration = .1f;
    public EventReference jumpscareSound;

    private GameObject _player;
    private FirstPersonController _playerController;
    private Jumpscare _jumpscare;
    private Transform _jumpscareFacePoint;

    private bool _triggered = false;


    void Start()
    {
        if (GetComponent<MeshRenderer>() != null)
            GetComponent<MeshRenderer>().enabled = false;

        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<FirstPersonController>();
        _jumpscare = _jumpscareObject.GetComponent<Jumpscare>();
        _jumpscareFacePoint = _jumpscare.transform.Find("FacePoint");
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject == _player) && !_triggered)
        {
            TriggerJumpscare();
            OnJumpscareSound();
            transform.GetComponent<Collider>().enabled = false;
        }
    }

    private void TriggerJumpscare()
    {
        _player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        if (!_triggered)
        {
            _triggered = true;
            _playerController.DisableInput();
            _jumpscare.Scare();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(StartJumpscare());

            UnityEngine.Debug.Log("Jumpscare!");

        }
    }

    private IEnumerator StartJumpscare()
    {
        yield return new WaitForSeconds(0.2f);
        if (_jumpscare.GetComponent<JumpscareSpawn>()._isPlaced)
        {
            if (_jumpscare.GetType() == typeof(JumpscareSpawn))
            {
                _jumpscare.transform.GetComponent<CapsuleCollider>().enabled = false;
                Vector3 lookDirection = (_jumpscareFacePoint.position - _playerController.transform.position).normalized;
                lookDirection.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                _playerController.transform.DORotateQuaternion(targetRotation, _tweenDuration);

                Vector3 cameraLookDirection = (_jumpscareFacePoint.position - _playerController.playerCamera.transform.position).normalized;
                cameraLookDirection.y = 0;
                Quaternion cameraTargetRotation = Quaternion.LookRotation(cameraLookDirection);
                _playerController.playerCamera.transform.DORotateQuaternion(cameraTargetRotation, _tweenDuration);
            }

            yield return new WaitForSeconds(_durationUntilTalk);
            _jumpscare.transform.GetComponent<CapsuleCollider>().enabled = true;
            _jumpscare.transform.GetComponent<NPCBaseController>().Interact();
            yield return 0;
        }
    }

    private void OnJumpscareSound()
    {
        EventInstance soundOnInteract = RuntimeManager.CreateInstance(jumpscareSound);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    }

}
