using DG.Tweening;
using System.Collections;
using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;

public class JumpscareInteract : MonoBehaviour, IInteractable
{
    [Header("Jumpscare Type")]
    [SerializeField] private bool barnGates = false;
    [SerializeField] private bool leaderRoom = false;

    [Header("Jumpscare Variables")]
    [SerializeField] private Jumpscare _jumpscareObject;
    [SerializeField] private float _durationUntilTalk;
    [SerializeField] private float _tweenDuration = .1f;
    [SerializeField] private GameObject Monolouge;
    [SerializeField] private string actionName;
    public EventReference jumpscareSound;
    private bool _hasActivatedJumpscare;
    private GameObject _player;
    private FirstPersonController _playerController;
    private Jumpscare _jumpscare;
    private Transform _jumpscareFacePoint;
    

    private bool _triggered = false;
    private float _timeElapsed = 0;

    
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<FirstPersonController>();
        _jumpscare = _jumpscareObject.GetComponent<Jumpscare>();
        _jumpscareFacePoint = _jumpscare.transform.Find("FacePoint");
        _hasActivatedJumpscare = _player.GetComponentInChildren<StaticStateManager>().getHasActivatedGateJumpsscare();

    }

    void Update()
    {
        
    }

    private void TriggerJumpscare()
    {
        if (!_triggered)
        {
            PlayInteractSound();
            _triggered = true;
            _playerController.DisableInput();
            _jumpscare.Scare();

            StartCoroutine(StartJumpscare());

            UnityEngine.Debug.Log("Jumpscare!");

        }
    }

    public void Interact()
    {
        if (leaderRoom && InventoryManager.Instance.inventoryItems.Contains("Leader Room Key"))
        {
            TriggerJumpscare();
        }
        else if (barnGates && !_hasActivatedJumpscare)
        {
            _player.GetComponentInChildren<StaticStateManager>().setHasActivatedGateJumpsscare(true);
            _hasActivatedJumpscare = true;
            TriggerJumpscare();
        }
        else if (barnGates && _hasActivatedJumpscare)
        {
            Monolouge.GetComponent<LookScript>().Interact();   
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


    public void Activate()
    {
        //throw new System.NotImplementedException();
    }

    public void Deactivate()
    {
        //throw new System.NotImplementedException();
    }

    public string GetActionName()
    {
        return actionName;
    }

    public string GetName()
    {
        return " ";
    }

    public void PlayInteractSound()
    {
        EventInstance jumpscareSoundInstance = RuntimeManager.CreateInstance(jumpscareSound);
        RuntimeManager.AttachInstanceToGameObject(jumpscareSoundInstance, transform);
        jumpscareSoundInstance.start();
        jumpscareSoundInstance.release();
    }
}
